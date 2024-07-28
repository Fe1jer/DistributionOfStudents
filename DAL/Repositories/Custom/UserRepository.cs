using DAL.Context;
using DAL.Entities;
using DAL.Context;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Custom
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext appDBContext) : base(appDBContext) { }


        public async Task<User?> GetByUrlAsync(string username)
        {
            return await EntitySet.SingleOrDefaultAsync(i => i.UserName == username);
        }
    }
}
