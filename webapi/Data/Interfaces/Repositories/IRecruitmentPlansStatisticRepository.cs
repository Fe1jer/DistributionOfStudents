using webapi.Data.Models;
using webapi.Data.Specifications.Base;

namespace webapi.Data.Interfaces.Repositories
{
    public interface IRecruitmentPlansStatisticRepository
    {
        Task<RecruitmentPlanStatistic?> GetByPlanAndDateAsync(int planId, DateTime date);
        Task<RecruitmentPlanStatistic?> GetByIdAsync(int statisticId);
        Task<List<RecruitmentPlanStatistic>> GetAllAsync();
        Task<List<RecruitmentPlanStatistic>> GetAllAsync(int planId);
        Task AddAsync(RecruitmentPlanStatistic statistic);
        Task UpdateAsync(RecruitmentPlanStatistic statistic);
        Task DeleteAsync(int id);
    }
}
