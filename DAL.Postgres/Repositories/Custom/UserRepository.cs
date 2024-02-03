using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;
using Microsoft.EntityFrameworkCore;

namespace DAL.Postgres.Repositories.Custom
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext appDBContext) : base(appDBContext) { }


        public async Task<User?> GetByUrlAsync(string username)
        {
            return await EntitySet.SingleOrDefaultAsync(i => i.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
