using BLL.DTO.User;

namespace BLL.Services.Interfaces
{
    public interface IUserTokenService
    {
        Task<object?> AuthenticateAsync(LoginDTO model, string? deviceInfo = null, string? ipAddress = null);
        Task<object> RefreshAsync(string refreshToken);
        Task<object?> SignOutAsync(string refreshToken);
    }
}
