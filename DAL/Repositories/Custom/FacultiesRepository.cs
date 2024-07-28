using DAL.Context;
using DAL.Entities;
using DAL.Context;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;
using DAL.Specifications.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Custom
{
    public class FacultiesRepository : Repository<Faculty>, IFacultiesRepository
    {
        public FacultiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }
        public async Task<int> GetCountByUrlAsync(string newUrl, string oldUrl)
        {
            return await EntitySet.CountAsync(p => p.ShortName == newUrl && p.ShortName != oldUrl);
        }

        public async Task<Faculty?> GetByUrlAsync(string url)
        {
            return await EntitySet.SingleOrDefaultAsync(i => i.ShortName == url);
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
