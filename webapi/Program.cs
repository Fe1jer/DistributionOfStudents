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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Выбор подключения к БД
            string connection = builder.Configuration.GetConnectionString("NpgsqlConnection")!;
            Logger.Info(connection);
            builder.Services.RegisterApplicationServices(connection);

            ConfigureServices(builder);

            var app = builder.Build();
            ConfigurePipeline(app);

            builder.Services.InitDatabase();

            app.Run();
        }

        // Регистрация сервисов
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddMemoryCache();

            // CORS — именованная политика
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<LinkGeneratorHelper>();

            // JWT
            var accessTokenSettings = builder.Configuration.GetSection("AccessTokenSettings")!;
            builder.Services.AddJwtAuthentication(accessTokenSettings);

            // NLog
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Host.UseNLog();
        }

        // Конфигурация middleware и пайплайна
        public static void ConfigurePipeline(WebApplication app)
        {
            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

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

            app.MapControllers();
        }
    }
}