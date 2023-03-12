using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Data.Specifications.Base;

namespace DistributionOfStudents.Data.Interfaces
{
    public interface IRecruitmentPlansRepository
    {
        Task<RecruitmentPlan?> GetByIdAsync(int recruitmentPlanId);
        Task<RecruitmentPlan?> GetByIdAsync(int recruitmentPlanId, ISpecification<RecruitmentPlan> specification);
        Task<List<RecruitmentPlan>> GetAllAsync();
        Task<List<RecruitmentPlan>> GetAllAsync(ISpecification<RecruitmentPlan> specification);
        Task AddAsync(RecruitmentPlan recruitmentPlan);
        Task UpdateAsync(RecruitmentPlan recruitmentPlan);
        Task DeleteAsync(int id);
    }
}
