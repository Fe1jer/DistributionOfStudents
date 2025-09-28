using DAL.Context;
using DAL.Entities.Base;
using DAL.Repositories.Interfaces.Base;
using DAL.Specifications;
using DAL.Specifications.Base;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
namespace DAL.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected AuthDbContext Context { get; set; }
        protected DbSet<T> EntitySet => Context.Set<T>();
        protected IDbConnection Connection => Context.Database.GetDbConnection();

        public Repository(AuthDbContext appDbContext)
        {
            Context = appDbContext;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await EntitySet.Where(predicate).CountAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            EntitySet.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await EntitySet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await EntitySet.FindAsync(id);
        }

        public async Task<T?> GetByIdAsync(Guid id, ISpecification<T> specification)
        {
            return await ApplySpecification(specification).SingleOrDefaultAsync(it => it.Id == id);
        }

        public async Task InsertOrUpdateAsync(T entity)
        {
            if (entity.IsNew)
                await Context.AddAsync(entity);
            else
            {
                EntitySet.Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public async Task<List<T>> GetAllAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        protected IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator.ApplySpecification(Context.Set<T>().AsQueryable(), spec);
        }
    }
}
