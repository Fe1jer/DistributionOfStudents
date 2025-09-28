using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Helpers;
using System.Text;

namespace Shared.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfigurationSection jwtSettingsConfiguration)
        {
            // JWT
            services.Configure<AccessTokenSettings>(jwtSettingsConfiguration);
            var jwtSettings = jwtSettingsConfiguration.Get<AccessTokenSettings>();

            var publicKey = Encoding.UTF8.GetBytes(jwtSettings.PublicKey);
            var symmetricKey = new SymmetricSecurityKey(publicKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = symmetricKey,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            });

            return services;
        }
    }
}
