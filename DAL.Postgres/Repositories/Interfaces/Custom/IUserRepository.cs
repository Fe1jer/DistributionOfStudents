using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task<User?> GetByNameAsync(string username);
        Task<List<User>> GetAllAsync();
        Task<List<User>> GetAllAsync(ISpecification<User> specification);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
