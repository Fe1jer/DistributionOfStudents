using BLL.DTO;
using BLL.DTO.RecruitmentPlans;

namespace BLL.Services.Interfaces
{
    public interface IDistributionService
    {
        public Task<float> GetCompetitionAsync(string facultyUrl, Guid groupId);
        public Task<Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>>> GetAsync(string facultyUrl, Guid groupId);
        public Task<Dictionary<RecruitmentPlanDTO, List<AdmissionDTO>>> CreateAsync(string facultyUrl, Guid groupId, List<PlanForDistributionDTO> models);
        public Task<bool> ExistsEnrolledStudentsAsync(string facultyUrl, Guid groupId);
        Task SaveAsync(string facultyUrl, Guid groupId, List<PlanForDistributionDTO> models, bool notify);
        Task DeleteAsync(string facultyUrl, Guid groupId);
    }
}
