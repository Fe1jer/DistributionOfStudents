using DependencyResolver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Helpers;
using System.Text;
using webapi.Data.Interfaces.Services;
using webapi.Data.Services;

namespace webapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();

            Configure(app);
            builder.Services.InitDatabase();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            //MSSQL подключение
            /*builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));*/
            string connection = builder.Configuration.GetConnectionString("NpgsqlConnection");
            builder.Services.RegisterApplicationServices(connection);

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
            var authkey = builder.Configuration.GetValue<string>("AppSettings:Secret");
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authkey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddSingleton<LinkGeneratorHelper>();
            // configure DI for application services
            builder.Services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

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

            app.Run();
        }
    }
}