namespace BLL.Services;

using BLL.DTO.User;
using BLL.Services.Base;
using BLL.Services.Interfaces;
using ChartJSCore.Models;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class UserService : BaseService, IUserService
{
    private readonly JWTSettings _setting;

    public UserService(IUnitOfWork unitOfWork, IOptions<JWTSettings> options) : base(unitOfWork)
    {
        _setting = options.Value;
    }

    public async Task<object?> Authenticate(LoginDTO model)
    {
        User? _user = await _unitOfWork.Users.GetByUrlAsync(model.Username);
        if (_user == null)
            return null;
        if (!VerifyHashedPassword(_user.PasswordHash, model.Password))
            return null;

        return new { jwtToken = GenerateJwtToken(_user), user = _user };
    }

    public async Task<UserDTO?> Registration(RegisterDTO model)
    {
        if (await _unitOfWork.Users.GetByUrlAsync(model.UserName) != null) return null;

        User entity = Mapper.Map<User>(model);
        entity.PasswordHash = GetHashPassword(model.Password);
        entity.Role = "commission";

        if (model.FileImg != null)
        {
            string path = "\\img\\Users\\" + entity.UserName + "_" + model.FileImg.FileName;
            entity.Img = IFileService.UploadFile(model.FileImg, path);
        }
        await _unitOfWork.Users.InsertOrUpdateAsync(entity);

        return Mapper.Map<UserDTO>(entity);
    }

    public async Task<List<UserDTO>> GetAllAsync()
    {
        var result = await _unitOfWork.Users.GetAllAsync();
        return Mapper.Map<List<UserDTO>>(result);
    }

    public async Task<UserDTO> GetAsync(Guid id)
    {
        var entity = await _unitOfWork.Users.GetByIdAsync(id);
        return Mapper.Map<UserDTO>(entity);
    }

    public async Task<UserDTO> GetAsync(string userName)
    {
        var entity = await _unitOfWork.Users.GetByUrlAsync(userName);
        return Mapper.Map<UserDTO>(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var toDelete = await _unitOfWork.Users.GetByIdAsync(id);
        await _unitOfWork.Users.DeleteAsync(toDelete);
        _unitOfWork.Commit();
    }

    public async Task<UserDTO?> SaveAsync(UserDTO model)
    {
        User? entity;
        if (model.IsNew)
        {
            if (await _unitOfWork.Users.GetByUrlAsync(model.UserName) != null) return null;
            entity = Mapper.Map<User>(model);
            entity.Role = "commission";
        }
        else
        {
            entity = await _unitOfWork.Users.GetByIdAsync(model.Id);
            Mapper.Map(model, entity);
        }
        if (!string.IsNullOrEmpty(model.Password))
        {
            entity.PasswordHash = GetHashPassword(model.Password);
        }
        if (model.FileImg != null)
        {
            string path = "\\img\\Users\\" + entity.UserName + "_" + model.FileImg.FileName;
            if (entity.Img != "\\img\\Users\\bntu.jpg")
            {
                IFileService.DeleteFile(entity.Img);
            }
            entity.Img = IFileService.UploadFile(model.FileImg, path);
        }

        await _unitOfWork.Users.InsertOrUpdateAsync(entity);
        _unitOfWork.Commit();

        return Mapper.Map<UserDTO>(entity);
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
                        new(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                        new(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                }
            ),
            Expires = DateTime.UtcNow.AddMinutes(1),
            NotBefore = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenhandler.CreateToken(tokenDescriptor);
        return tokenhandler.WriteToken(token);
    }
}