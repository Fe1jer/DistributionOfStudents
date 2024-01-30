using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;
using DAL.Postgres.Specifications;

namespace DAL.Postgres.Repositories.Custom
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext appDBContext) : base(appDBContext) { }

        public async Task DeleteAsync(Guid id)
        {
            User? user = await GetByIdAsync(id);
            if (user != null)
            {
                await DeleteAsync(user);
            }
        }

        public async Task<User?> GetByNameAsync(string username)
        {
            List<User> users = await GetAllAsync(new UsersSpecification().WhereUserName(username));

            return users.FirstOrDefault();
        }
    }
}
