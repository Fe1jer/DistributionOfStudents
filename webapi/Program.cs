using DI;
using NLog;
using NLog.Web;
using Shared.Extensions;
using Shared.Helpers;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace webapi
{
    public class Program
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //MSSQL подключение
            /*string connection = builder.Configuration.GetConnectionString("MssqlConnection");
            builder.Services.RegisterApplicationServices(connection);*/

            string connection = builder.Configuration.GetConnectionString("NpgsqlConnection")!;
            Logger.Info(connection);
            builder.Services.RegisterApplicationServices(connection);

            SetBuilderServices(builder);

            var app = builder.Build();
            SetApps(app);

            builder.Services.InitDatabase();

            app.Run();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void SetBuilderServices(WebApplicationBuilder builder)
        {
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
            var accessTokenSettings = builder.Configuration.GetSection("AccessTokenSettings")!;
            builder.Services.AddJwtAuthentication(accessTokenSettings);

            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Host.UseNLog();

            builder.Services.AddSingleton<LinkGeneratorHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void SetApps(WebApplication app)
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

            app.UseExceptionHandler();
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
            }
        }
    }
}