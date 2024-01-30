using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;

namespace DAL.Postgres.Repositories.Custom
{
    public class FormsOfEducationRepository : Repository<FormOfEducation>, IFormsOfEducationRepository
    {
        public FormsOfEducationRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(Guid id)
        {
            FormOfEducation? form = await GetByIdAsync(id);
            if (form != null)
            {
                await DeleteAsync(form);
            }
        }
    }
}
