using DAL.Postgres.Entities;
using DAL.Postgres.Entities.Base;
using DAL.Postgres.Specifications.Base;
using System.Linq.Expressions;

namespace DAL.Postgres.Repositories.Interfaces.Base
{
    public interface IRepository<T> where T : Entity
    {
        public Task<List<T>> GetAllAsync(ISpecification<T> specification);
        public Task<List<T>> GetAllAsync();
        Task InsertOrUpdateAsync(T entity);
        public Task DeleteAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByIdAsync(Guid id, ISpecification<T> specification);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}
