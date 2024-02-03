using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces.Base;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUrlAsync(string username);
    }
}
