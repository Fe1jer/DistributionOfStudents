using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;

namespace DAL.Postgres.Repositories.Custom
{
    public class RecruitmentPlansRepository : Repository<RecruitmentPlan>, IRecruitmentPlansRepository
    {
        public RecruitmentPlansRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(Guid id)
        {
            RecruitmentPlan? recruitmentPlan = await GetByIdAsync(id);
            if (recruitmentPlan != null)
            {
                await DeleteAsync(recruitmentPlan);
            }
        }
    }
}
