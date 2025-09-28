using BLL.Services.Interfaces;
using DAL.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.Services
{
    public class JwtService : IJwtService
    {
        private readonly IOptions<AccessTokenSettings> _settings;

        public JwtService(IOptions<AccessTokenSettings> settings)
        {
            _settings = settings;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(_settings.Value.PublicKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                        new(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(1),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256),
                Issuer = _settings.Value.Issuer,
                Audience = _settings.Value.Audience,
            };
            var token = tokenhandler.CreateToken(tokenDescriptor);
            return tokenhandler.WriteToken(token);
        }
    }
}
