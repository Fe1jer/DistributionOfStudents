using webapi.Data.Interfaces.Repositories;
using webapi.Data.Models;
using webapi.Data.Repositories.Base;

namespace webapi.Data.Repositories
{
    public class FormsOfEducationRepository : Repository<FormOfEducation>, IFormsOfEducationRepository
    {
        public FormsOfEducationRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            FormOfEducation? form = await GetByIdAsync(id);
            if (form != null)
            {
                await DeleteAsync(form);
            }
        }
    }
}
