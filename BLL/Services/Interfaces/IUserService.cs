using BLL.DTO.User;
using DAL.Postgres.Entities;

namespace BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<object?> Authenticate(LoginDTO model);
        Task<User?> Registration(RegisterDTO model);
    }
}
