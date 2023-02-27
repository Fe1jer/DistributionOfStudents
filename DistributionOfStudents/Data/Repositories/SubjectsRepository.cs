using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories.Base;

namespace DistributionOfStudents.Data.Repositories
{
    public class SubjectsRepository : Repository<Subject>, ISubjectsRepository
    {
        public SubjectsRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            Subject subject = await GetByIdAsync(id);
            await DeleteAsync(subject);
        }
    }
}
