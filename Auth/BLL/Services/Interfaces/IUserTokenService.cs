using BLL.DTO;
using BLL.DTO.User;

namespace BLL.Services.Interfaces
{
    public interface IUserTokenService
    {
        Task<object?> AuthenticateAsync(LoginDTO model, RequestContextDTO? context = null);
        Task<object> RefreshAsync(string refreshToken);
        Task<object?> SignOutAsync(string refreshToken);
    }
}
