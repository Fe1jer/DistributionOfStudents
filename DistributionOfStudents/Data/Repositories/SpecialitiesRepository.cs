using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories.Base;

namespace DistributionOfStudents.Data.Repositories
{
    public class SpecialitiesRepository : Repository<Speciality>, ISpecialitiesRepository
    {
        public SpecialitiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            Speciality speciality = await GetByIdAsync(id);
            await DeleteAsync(speciality);
        }
    }
}
