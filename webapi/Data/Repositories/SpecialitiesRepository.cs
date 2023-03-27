using webapi.Data.Interfaces.Repositories;
using webapi.Data.Models;
using webapi.Data.Repositories.Base;

namespace webapi.Data.Repositories
{
    public class SpecialitiesRepository : Repository<Speciality>, ISpecialitiesRepository
    {
        public SpecialitiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            Speciality? speciality = await GetByIdAsync(id);
            if (speciality != null)
            {
                await DeleteAsync(speciality);
            }
        }
    }
}
