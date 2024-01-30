using DAL.Postgres.Context;
using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Base;
using DAL.Postgres.Repositories.Interfaces.Custom;

namespace DAL.Postgres.Repositories.Custom
{
    public class RecruitmentPlansStatisticRepository : Repository<RecruitmentPlanStatistic>, IRecruitmentPlansStatisticRepository
    {
        public RecruitmentPlansStatisticRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(Guid id)
        {
            RecruitmentPlanStatistic? statistic = await GetByIdAsync(id);
            if (statistic != null)
            {
                await DeleteAsync(statistic);
            }
        }

        public async Task<List<RecruitmentPlanStatistic>> GetAllAsync(Guid planId)
        {
            return (await GetAllAsync()).Where(i => i.RecruitmentPlan.Id == planId).OrderBy(i => i.Date).ToList();
        }

        public async Task<RecruitmentPlanStatistic?> GetByPlanAndDateAsync(Guid planId, DateTime date)
        {
            return (await GetAllAsync()).SingleOrDefault(i => i.RecruitmentPlan.Id == planId && i.Date == date);
        }
    }
}
