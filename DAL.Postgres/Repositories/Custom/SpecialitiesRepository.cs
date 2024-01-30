using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;

namespace DAL.Postgres.Repositories.Custom
{
    public class SpecialitiesRepository : Repository<Speciality>, ISpecialitiesRepository
    {
        public SpecialitiesRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
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
