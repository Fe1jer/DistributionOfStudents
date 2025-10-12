using DAL.Context;
using DAL.Entities;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces.Custom;

namespace DAL.Repositories.Custom
{
    public class RecruitmentPlansStatisticRepository(ApplicationDbContext appDBContext) : Repository<RecruitmentPlanStatistic>(appDBContext), IRecruitmentPlansStatisticRepository
    {
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
            return [.. (await GetAllAsync()).Where(i => i.RecruitmentPlanId == planId).OrderBy(i => i.Date)];
        }

        public async Task<RecruitmentPlanStatistic?> GetByPlanAndDateAsync(Guid planId, DateTime date)
        {
            return (await GetAllAsync()).SingleOrDefault(i => i.RecruitmentPlanId == planId && i.Date.ToLocalTime().Date == date.Date);
        }
    }
}
