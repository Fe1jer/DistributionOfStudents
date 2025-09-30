using DAL.Entities;

namespace BLL.Services.Interfaces
{
    public interface IJwtService
    {
        public Task<string> GenerateAccessTokenAsync(User user);
        public Task<string> GenerateRefreshTokenAsync();
        public Task<int> GetRefreshTokenLifetimeInDays();
    }
}
