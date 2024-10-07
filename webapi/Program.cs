using DI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using Shared.Helpers;
using System.Text;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace webapi
{
    public class Program
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();

            Configure(app);
            builder.Services.InitDatabase();
            Logger.Info("Application started");

            app.Run();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            //MSSQL подключение
            string connection = builder.Configuration.GetConnectionString("MssqlConnection");
            Logger.Info(connection);
            builder.Services.RegisterApplicationServices(connection);
/*
            string connection = builder.Configuration.GetConnectionString("NpgsqlConnection")!;
            builder.Services.RegisterApplicationServices(connection);*/

            builder.Services.AddCors();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerGen();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddCors();

            // configure strongly typed settings object
            builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("AppSettings"));
            var authKey = builder.Configuration.GetValue<string>("AppSettings:Secret");
            builder.Services.AddAuthentication(item =>
            {
                item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(item =>
            {
                item.RequireHttpsMetadata = true;
                item.SaveToken = true;
                item.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Host.UseNLog();

            builder.Services.AddSingleton<LinkGeneratorHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler(new ExceptionHandlerOptions()
                {
                    ExceptionHandler = context =>
                    {
                        var ex = context.Features.Get<IExceptionHandlerFeature>();

                        if (ex != null)
                        {
                            Logger.Error(ex);
                        }

                        return Task.CompletedTask;
                    }
                });

                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDefaultFiles();
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseHttpsRedirection();

            // configure HTTP request pipeline
            {
                // global cors policy
                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

                // custom jwt auth middleware
                app.UseMiddleware<JwtMiddleware>();
            }
        }
    }
}