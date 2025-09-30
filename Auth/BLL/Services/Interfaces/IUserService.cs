using BLL.DTO.User;

namespace BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync();
        Task<UserDTO> GetAsync(Guid id);
        Task<UserDTO> GetAsync(string userName);
        Task DeleteAsync(Guid id);
        Task<UserDTO?> SaveAsync(UserDTO model);
        Task<UserDTO?> Registration(RegisterDTO model);
    }
}
