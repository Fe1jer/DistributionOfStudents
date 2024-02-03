using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;
using DAL.Postgres.Specifications.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Postgres.Repositories.Custom
{
    public class FacultiesRepository : Repository<Faculty>, IFacultiesRepository
    {
        public FacultiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }
        public async Task<int> GetCountByUrlAsync(string url, Guid excludeId)
        {
            return await EntitySet.CountAsync(p => p.ShortName.Equals(url, StringComparison.OrdinalIgnoreCase) && p.Id != excludeId);
        }

        public async Task<Faculty?> GetByUrlAsync(string url)
        {
            return await EntitySet.SingleOrDefaultAsync(i => i.ShortName.Equals(url, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Faculty?> GetByUrlAsync(string url, ISpecification<Faculty> specification)
        {
            Faculty? entity = await GetByUrlAsync(url);
            if (entity != null)
            {
                return await GetByIdAsync(entity.Id, specification);
            }

            return entity;
        }
    }
}
