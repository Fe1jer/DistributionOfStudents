namespace webapi.Data.Services;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using webapi.Data.Models;
using webapi.Helpers;
using webapi.ViewModels;
using webapi.Data.Interfaces.Services;
using webapi.Data.Interfaces.Repositories;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly JWTSettings _setting;

    public UserService(IUserRepository userRepository, IOptions<JWTSettings> options)
    {
        _userRepository = userRepository;
        _setting = options.Value;
    }

    public async Task<object?> Authenticate(LoginVM model)
    {
        User ? _user = await _userRepository.GetByNameAsync(model.Username);
        if (_user == null)
            return null;
        if (!VerifyHashedPassword(_user.PasswordHash, model.Password))
            return null;

        return new { jwtToken = GenerateJwtToken(_user), user = _user };
    }

    private static string GetHashPassword(string password)
    {
        byte[] salt;
        byte[] buffer2;
        if (password == null)
        {
            throw new ArgumentNullException(nameof(password));
        }
        using (Rfc2898DeriveBytes bytes = new(password, 0x10, 0x3e8))
        {
            salt = bytes.Salt;
            buffer2 = bytes.GetBytes(0x20);
        }
        byte[] dst = new byte[0x31];
        Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
        Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
        return Convert.ToBase64String(dst);
    }

    private static bool VerifyHashedPassword(string hashedPassword, string password)
    {
        byte[] buffer4;
        if (hashedPassword == null)
        {
            return false;
        }
        if (password == null)
        {
            throw new ArgumentNullException(nameof(password));
        }
        byte[] src = Convert.FromBase64String(hashedPassword);
        if (src.Length != 0x31 || src[0] != 0)
        {
            return false;
        }
        byte[] dst = new byte[0x10];
        Buffer.BlockCopy(src, 1, dst, 0, 0x10);
        byte[] buffer3 = new byte[0x20];
        Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
        using (Rfc2898DeriveBytes bytes = new(password, dst, 0x3e8))
        {
            buffer4 = bytes.GetBytes(0x20);
        }
        return ByteArraysEqual(buffer3, buffer4);
    }

    private static bool ByteArraysEqual(byte[] b1, byte[] b2)
    {
        if (b1 == b2)
        {
            return true;
        }

        if (b1 == null || b2 == null || b1.Length != b2.Length)
        {
            return false;
        }

        for (int i = 0; i < b1.Length; i++)
        {
            if (b1[i] != b2[i])
            {
                return false;
            }
        }
        return true;
    }

    // helper methods

    private string GenerateJwtToken(User user)
    {
        var tokenhandler = new JwtSecurityTokenHandler();
        var tokenkey = Encoding.UTF8.GetBytes(_setting.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                }
            ),
            Expires = DateTime.Now.AddMinutes(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenhandler.CreateToken(tokenDescriptor);
        return tokenhandler.WriteToken(token);
    }
}