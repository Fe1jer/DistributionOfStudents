using DAL.Entities;
using DAL.Repositories.Interfaces.Base;

namespace DAL.Repositories.Interfaces.Custom
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUserNameAsync(string username);
    }
}
