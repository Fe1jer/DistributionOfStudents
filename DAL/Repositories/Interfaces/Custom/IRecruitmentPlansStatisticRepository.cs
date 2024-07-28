using DAL.Entities;
using DAL.Repositories.Interfaces.Base;

namespace DAL.Repositories.Interfaces.Custom
{
    public interface IRecruitmentPlansStatisticRepository : IRepository<RecruitmentPlanStatistic>
    {
        Task<RecruitmentPlanStatistic?> GetByPlanAndDateAsync(Guid planId, DateTime date);
        Task<List<RecruitmentPlanStatistic>> GetAllAsync(Guid planId);
    }
}
