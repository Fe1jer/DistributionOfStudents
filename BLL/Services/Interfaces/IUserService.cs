using BLL.DTO;
using BLL.DTO.User;
using DAL.Postgres.Entities;

namespace BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync();
        Task<UserDTO> GetAsync(Guid id);
        Task<UserDTO> GetAsync(string userName);
        Task DeleteAsync(Guid id);
        Task<UserDTO?> SaveAsync(UserDTO model);
        Task<object?> Authenticate(LoginDTO model);
        Task<UserDTO?> Registration(RegisterDTO model);
    }
}
