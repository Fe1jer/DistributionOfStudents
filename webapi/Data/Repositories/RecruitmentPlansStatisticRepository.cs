using ChartJSCore.Models;
using webapi.Data.Models;
using webapi.Data.Repositories.Base;
using System.Text.RegularExpressions;
using webapi.Data.Interfaces.Repositories;

namespace webapi.Data.Repositories
{
    public class RecruitmentPlansStatisticRepository : Repository<RecruitmentPlanStatistic>, IRecruitmentPlansStatisticRepository
    {
        public RecruitmentPlansStatisticRepository(ApplicationDbContext appDBContext) : base(appDBContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            RecruitmentPlanStatistic? statistic = await GetByIdAsync(id);
            if (statistic != null)
            {
                await DeleteAsync(statistic);
            }
        }

        public async Task<List<RecruitmentPlanStatistic>> GetAllAsync(int planId)
        {
            return (await GetAllAsync()).Where(i => i.RecruitmentPlan.Id == planId).OrderBy(i => i.Date).ToList();
        }

        public async Task<RecruitmentPlanStatistic?> GetByPlanAndDateAsync(int planId, DateTime date)
        {
            return (await GetAllAsync()).SingleOrDefault(i => i.RecruitmentPlan.Id == planId && i.Date == date);
        }
    }
}
