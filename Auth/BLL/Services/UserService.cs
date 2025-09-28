namespace BLL.Services;

using BLL.DTO.User;
using BLL.Services.Base;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;

public class UserService : BaseService, IUserService
{
    private readonly ICryptographyService _cryptographyService;
    private readonly IJwtService _jwtService;

    public UserService(IUnitOfWork unitOfWork, ICryptographyService cryptographyService, IJwtService jwtService) : base(unitOfWork)
    {
        _cryptographyService = cryptographyService;
        _jwtService = jwtService;
    }

    public async Task<object?> Authenticate(LoginDTO model)
    {
        User? _user = await _unitOfWork.Users.GetByUrlAsync(model.Username);
        if (_user == null)
            return null;
        if (!_cryptographyService.VerifyHashedPassword(_user.PasswordHash, model.Password))
            return null;

        return new { jwtToken = _jwtService.GenerateJwtToken(_user), user = _user };
    }

    public async Task<UserDTO?> Registration(RegisterDTO model)
    {
        if (await _unitOfWork.Users.GetByUrlAsync(model.UserName) != null) return null;

        User entity = Mapper.Map<User>(model);
        entity.PasswordHash = _cryptographyService.GetHashPassword(model.Password);
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
            entity.PasswordHash = _cryptographyService.GetHashPassword(model.Password);
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
}