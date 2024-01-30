using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Custom
{
    public class FacultiesRepository : Repository<Faculty>, IFacultiesRepository
    {
        public FacultiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(Guid id)
        {
            Faculty? faculty = await GetByIdAsync(id);
            if (faculty != null)
            {
                await DeleteAsync(faculty);
            }
        }

        public async Task<Faculty?> GetByShortNameAsync(string name)
        {
            return (await GetAllAsync()).FirstOrDefault(i => i.ShortName.ToLower() == name.ToLower());
        }

        public async Task<Faculty?> GetByShortNameAsync(string name, ISpecification<Faculty> specification)
        {
            Faculty? faculty = await GetByShortNameAsync(name);
            if (faculty != null)
            {
                return await GetByIdAsync(faculty.Id, specification);
            }

            return faculty;
        }
    }
}
