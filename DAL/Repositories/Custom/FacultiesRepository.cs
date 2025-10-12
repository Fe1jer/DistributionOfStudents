using DAL.Context;
using DAL.Entities;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;
using DAL.Specifications.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Custom
{
    public class FacultiesRepository(ApplicationDbContext appDBContext) : Repository<Faculty>(appDBContext), IFacultiesRepository
    {
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
