using webapi.Data.Interfaces.Repositories;
using webapi.Data.Models;
using webapi.Data.Repositories.Base;

namespace webapi.Data.Repositories
{
    public class RecruitmentPlansRepository : Repository<RecruitmentPlan>, IRecruitmentPlansRepository
    {
        public RecruitmentPlansRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            RecruitmentPlan? recruitmentPlan = await GetByIdAsync(id);
            if (recruitmentPlan != null)
            {
                await DeleteAsync(recruitmentPlan);
            }
        }
    }
}
