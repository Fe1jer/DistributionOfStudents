using Auth_Shared.Helpers;
using BLL.Services.Interfaces;
using DAL.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BLL.Services
{
    public class JwtService : IJwtService
    {
        private readonly IOptions<JwtSettings> _settings;
        private readonly RsaSecurityKey _rsaSecurityKey;

        public JwtService(IOptions<JwtSettings> settings, RsaSecurityKey rsaSecurityKey)
        {
            _settings = settings;
            _rsaSecurityKey = rsaSecurityKey;
        }

        public Task<string> GenerateAccessTokenAsync(User user)
        {
            var signingCredentials = new SigningCredentials(
                key: _rsaSecurityKey,
                algorithm: SecurityAlgorithms.RsaSha256
            );

            var claimsIdentity = new ClaimsIdentity();

            // Обязательный идентификатор пользователя
            claimsIdentity.AddClaims(
                new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Role, user.Role.ToString())
                }
            );

            var jwtHandler = new JwtSecurityTokenHandler();

            var jwt = jwtHandler.CreateJwtSecurityToken(
                issuer: _settings.Value.AccessTokenSettings.Issuer,
                audience: _settings.Value.AccessTokenSettings.Audience,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_settings.Value.AccessTokenSettings.LifeTimeInMinutes),
                issuedAt: DateTime.UtcNow,
                signingCredentials: signingCredentials
            );

            var serializedJwt = jwtHandler.WriteToken(jwt);

            return Task.FromResult(serializedJwt);
        }


        public Task<string> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[_settings.Value.RefreshTokenSettings.Length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Task.FromResult(Convert.ToBase64String(randomNumber));
        }

        public Task<int> GetRefreshTokenLifetimeInDays()
        {
            return Task.FromResult(_settings.Value.RefreshTokenSettings.LifeTimeInDays);
        }
    }
}
