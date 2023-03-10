using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories.Base;
using DistributionOfStudents.Data.Specifications;
using DistributionOfStudents.Data.Specifications.Base;
using System.Xml.Linq;

namespace DistributionOfStudents.Data.Repositories
{
    public class FacultiesRepository : Repository<Faculty>, IFacultiesRepository
    {
        public FacultiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            Faculty faculty = await GetByIdAsync(id);
            await DeleteAsync(faculty);
        }

        public async Task<Faculty> GetByShortNameAsync(string name)
        {
            return (await GetAllAsync()).FirstOrDefault(i => i.ShortName == name);
        }

        public async Task<Faculty> GetByShortNameAsync(string name, ISpecification<Faculty> specification)
        {
            Faculty faculty = (await GetAllAsync()).FirstOrDefault(i => i.ShortName == name);
            if (faculty != null)
            {
                return await GetByIdAsync(faculty.Id, specification);
            }

            return faculty;
        }
    }
}
