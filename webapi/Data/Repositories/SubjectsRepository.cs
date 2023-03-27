using webapi.Data.Interfaces.Repositories;
using webapi.Data.Models;
using webapi.Data.Repositories.Base;

namespace webapi.Data.Repositories
{
    public class SubjectsRepository : Repository<Subject>, ISubjectsRepository
    {
        public SubjectsRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            Subject? subject = await GetByIdAsync(id);
            if (subject != null)
            {
                await DeleteAsync(subject);
            }
        }
    }
}
