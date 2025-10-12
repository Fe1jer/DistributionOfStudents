using Auth_Shared.Helpers;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using Shared.Extensions;
using Shared.Helpers;
using System.Security.Cryptography;
using webapi.DI;
using webapi.Providers;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace webapi
{
    public class Program
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

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

            // HttpContextAccessor и провайдер контекста запроса
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<RequestContextProvider>();

            // JWT и ключи
            var jwtSettingsConfiguration = builder.Configuration.GetSection("JwtSettings")!;
            builder.Services.Configure<JwtSettings>(jwtSettingsConfiguration);
            var jwtSettings = jwtSettingsConfiguration.Get<JwtSettings>();
            var accessTokenSettings = jwtSettingsConfiguration.GetSection("AccessTokenSettings")!;
            builder.Services.AddJwtAuthentication(accessTokenSettings);

            builder.Services.AddSingleton(provider =>
            {
                var rsa = RSA.Create();
                rsa.ImportRSAPrivateKey(
                    source: Convert.FromBase64String(jwtSettings.AccessTokenSettings.PrivateKey),
                    bytesRead: out int _);
                return new RsaSecurityKey(rsa);
            });

            // NLog
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Host.UseNLog();

            builder.Services.AddSingleton<LinkGeneratorHelper>();
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
                // app.UseHsts();
            }

            app.MapControllers();
        }
    }
}
