namespace webapi.Helpers;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using webapi.Data.Interfaces.Repositories;
using webapi.Data.Models;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JWTSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<JWTSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserRepository userRepository)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            await AttachUserToContext(context, userRepository, token);

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, IUserRepository userRepository, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type.Equals("name")).Value);

            User? user = await userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                context.Items["User"] = user;
            }
            // attach user to context on successful jwt validation
        }
        catch
        {
            // do nothing if jwt validation fails
            // user is not attached to context so request won't have access to secure routes
        }
    }
}