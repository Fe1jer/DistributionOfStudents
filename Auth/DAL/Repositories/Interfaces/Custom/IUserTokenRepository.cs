using DAL.Entities;
using DAL.Repositories.Interfaces.Base;

namespace DAL.Repositories.Interfaces.Custom
{
    public interface IUserTokenRepository : IRepository<UserToken>
    {
        Task<UserToken?> GetByTokenWithUserAsync(string refreshToken);
        Task<List<UserToken>> GetByUserIdAsync(Guid userId);
        Task<List<UserToken>> GetBySessionIdAsync(Guid userId, Guid sessionId);
    }
}
