using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;

namespace DistributionOfStudents.Data.Interfaces
{
    public interface IRecruitmentPlansStatisticRepository
    {
        Task<RecruitmentPlanStatistic?> GetByPlanAndDateAsync(int planId, DateTime date);
        Task<RecruitmentPlanStatistic?> GetByIdAsync(int statisticId);
        Task<List<RecruitmentPlanStatistic>> GetAllAsync();
        Task AddAsync(RecruitmentPlanStatistic statistic);
        Task UpdateAsync(RecruitmentPlanStatistic statistic);
        Task DeleteAsync(int id);
    }
}
