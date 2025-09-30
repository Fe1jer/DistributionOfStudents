using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Helpers;
using System.Security.Cryptography;

namespace Shared.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfigurationSection jwtSettingsConfiguration)
        {
            // JWT
            services.Configure<AccessTokenSettings>(jwtSettingsConfiguration);
            var jwtSettings = jwtSettingsConfiguration.Get<AccessTokenSettings>();

            RSA rsa = RSA.Create();
            rsa.ImportRSAPublicKey(
                source: Convert.FromBase64String(jwtSettings.PublicKey),
                bytesRead: out int _
            );

            var rsaKey = new RsaSecurityKey(rsa);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = rsaKey,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            });

            return services;
        }
    }
}
