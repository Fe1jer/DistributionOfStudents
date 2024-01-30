using DAL.Postgres.Entities;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IRecruitmentPlansStatisticRepository
    {
        Task<RecruitmentPlanStatistic?> GetByPlanAndDateAsync(Guid planId, DateTime date);
        Task<RecruitmentPlanStatistic?> GetByIdAsync(Guid statisticId);
        Task<List<RecruitmentPlanStatistic>> GetAllAsync();
        Task<List<RecruitmentPlanStatistic>> GetAllAsync(Guid planId);
        Task AddAsync(RecruitmentPlanStatistic statistic);
        Task UpdateAsync(RecruitmentPlanStatistic statistic);
        Task DeleteAsync(Guid id);
    }
}
