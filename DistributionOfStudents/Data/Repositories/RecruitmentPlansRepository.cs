using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories.Base;

namespace DistributionOfStudents.Data.Repositories
{
    public class RecruitmentPlansRepository : Repository<RecruitmentPlan>, IRecruitmentPlansRepository
    {
        public RecruitmentPlansRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            RecruitmentPlan recruitmentPlan = await GetByIdAsync(id);
            await DeleteAsync(recruitmentPlan);
        }
    }
}
