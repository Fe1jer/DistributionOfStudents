using DAL.Postgres.Entities;
using DAL.Postgres.Repositories.Interfaces.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IRecruitmentPlansStatisticRepository : IRepository<RecruitmentPlanStatistic>
    {
        Task<RecruitmentPlanStatistic?> GetByPlanAndDateAsync(Guid planId, DateTime date);
        Task<List<RecruitmentPlanStatistic>> GetAllAsync(Guid planId);
    }
}
