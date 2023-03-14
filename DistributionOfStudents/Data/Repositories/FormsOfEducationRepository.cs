using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories.Base;

namespace DistributionOfStudents.Data.Repositories
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
