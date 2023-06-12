using webapi.Data.Models;
using webapi.Data.Interfaces.Repositories;
using webapi.Data.Repositories.Base;
using webapi.Data.Specifications;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext appDBContext) : base(appDBContext) { }

        public async Task DeleteAsync(int id)
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
