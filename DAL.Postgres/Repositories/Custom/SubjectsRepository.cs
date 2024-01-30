using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;

namespace DAL.Postgres.Repositories.Custom
{
    public class SubjectsRepository : Repository<Subject>, ISubjectsRepository
    {
        public SubjectsRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(Guid id)
        {
            Subject? subject = await GetByIdAsync(id);
            if (subject != null)
            {
                await DeleteAsync(subject);
            }
        }
    }
}
