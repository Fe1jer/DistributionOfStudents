using DAL.Context;
using DAL.Entities;
using DAL.Context;
using DAL.Specifications;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;
using DAL.Specifications;
using DAL.Specifications.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Custom
{
    public class SpecialitiesRepository : Repository<Speciality>, ISpecialitiesRepository
    {
        public SpecialitiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task<int> GetCountByUrlAsync(string url, Guid excludeId)
        {
            return await EntitySet.CountAsync(p => p.FullName == url && p.Id != excludeId);
        }

        public async Task<Speciality?> GetByUrlAsync(string url)
        {
            return await EntitySet.SingleOrDefaultAsync(p => p.FullName == url);
        }

        public async Task<Speciality?> GetByUrlAsync(string url, ISpecification<Speciality> specification)
        {
            Speciality? entity = await GetByUrlAsync(url);
            if (entity != null)
            {
                return await GetByIdAsync(entity.Id, specification);
            }

            return entity;
        }

        public Task<List<Speciality>> GetByFacultyAsync(string facultyUrl, bool isDisable)
        {
            return GetAllAsync(new SpecialitiesSpecification(p => p.IsDisabled == isDisable).WhereFaculty(facultyUrl).SortByCode());
        }

        public async Task DeleteAsync(Guid id)
        {
            Speciality? speciality = await GetByIdAsync(id);
            if (speciality != null)
            {
                await DeleteAsync(speciality);
            }
        }
    }
}
