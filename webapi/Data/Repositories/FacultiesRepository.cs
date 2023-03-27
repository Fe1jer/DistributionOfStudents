using webapi.Data.Interfaces.Repositories;
using webapi.Data.Models;
using webapi.Data.Repositories.Base;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Repositories
{
    public class FacultiesRepository : Repository<Faculty>, IFacultiesRepository
    {
        public FacultiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
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
