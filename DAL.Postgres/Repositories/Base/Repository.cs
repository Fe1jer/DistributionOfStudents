using DAL.Postgres.Context;
using DAL.Postgres.Entities.Base;
using DAL.Postgres.Repositories.Interfaces.Base;
using DAL.Postgres.Specifications;
using DAL.Postgres.Specifications.Base;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace DAL.Postgres.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected ApplicationDbContext Context { get; set; }
        protected DbSet<T> EntitySet => Context.Set<T>();
        protected IDbConnection Connection => Context.Database.GetDbConnection();

        public Repository(ApplicationDbContext appDBContext)
        {
            Context = appDBContext;
        }

        public async Task AddAsync(T entity)
        {
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
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
            var all = await GetAllAsync(specification);

            return all.FirstOrDefault(it => it.Id == id);
        }

        public async Task UpdateAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
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
