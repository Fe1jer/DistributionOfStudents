using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories.Base;

namespace DistributionOfStudents.Data.Repositories
{
    public class AdmissionsRepository : Repository<Admission>, IAdmissionsRepository
    {
        public AdmissionsRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            Admission? admission = await GetByIdAsync(id);
            if (admission != null)
            {
                await DeleteAsync(admission);
            }
        }
    }
}
