using DAL.Postgres.Entities;
using DAL.Postgres.Specifications.Base;

namespace DAL.Postgres.Repositories.Interfaces.Custom
{
    public interface IRecruitmentPlansRepository
    {
        Task<RecruitmentPlan?> GetByIdAsync(Guid recruitmentPlanId);
        Task<RecruitmentPlan?> GetByIdAsync(Guid recruitmentPlanId, ISpecification<RecruitmentPlan> specification);
        Task<List<RecruitmentPlan>> GetAllAsync();
        Task<List<RecruitmentPlan>> GetAllAsync(ISpecification<RecruitmentPlan> specification);
        Task AddAsync(RecruitmentPlan recruitmentPlan);
        Task UpdateAsync(RecruitmentPlan recruitmentPlan);
        Task DeleteAsync(Guid id);
    }
}
