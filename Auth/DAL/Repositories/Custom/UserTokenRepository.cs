using DAL.Context;
using DAL.Entities;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Custom
{
    public class UserTokenRepository : Repository<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(AuthDbContext appDBContext) : base(appDBContext) { }

        public async Task<UserToken?> GetByTokenWithUserAsync(string refreshToken)
        {
            return await EntitySet.Include(i => i.User).SingleOrDefaultAsync(i => i.RefreshToken == refreshToken);
        }

        public Task<List<UserToken>> GetBySessionIdAsync(Guid userId, Guid sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserToken>> GetByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
