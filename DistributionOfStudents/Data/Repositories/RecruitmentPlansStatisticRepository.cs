using DistributionOfStudents.Data.Interfaces;
using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Repositories.Base;
using System.Text.RegularExpressions;

namespace DistributionOfStudents.Data.Repositories
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

        public async Task<RecruitmentPlanStatistic?> GetByPlanAndDateAsync(int planId, DateTime date)
        {
            return (await GetAllAsync()).SingleOrDefault(i => i.RecruitmentPlan.Id == planId && i.Date == date);
        }
    }
}
