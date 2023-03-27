using webapi.Data.AbstractClasses;
using webapi.Data.Specifications.Base;
using System.Linq.Expressions;

namespace webapi.Data.Interfaces.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        public Task<List<T>> GetAllAsync(ISpecification<T> specification);
        public Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdAsync(int id, ISpecification<T> specification);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}
