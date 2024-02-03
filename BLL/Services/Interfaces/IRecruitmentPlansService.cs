using BLL.DTO.RecruitmentPlans;

namespace BLL.Services.Interfaces
{
    public interface IRecruitmentPlansService
    {
        Task<List<RecruitmentPlanDTO>> GetAllAsync();
        Task<RecruitmentPlanDTO> GetAsync(Guid id);
        Task<List<FacultyRecruitmentPlanDTO>> GetLastFacultiesPlansAsync();
        Task<FacultyRecruitmentPlanDTO> GetLastByFacultyAsync(string facultyUrl);
        Task<List<RecruitmentPlanDTO>> GetByFacultyAsync(string facultyUrl, int year);
        Task<List<RecruitmentPlanDTO>> GetByGroupAsync(Guid groupId);
        Task DeleteAsync(string facultyUrl, int year);
        Task DeleteAsync(Guid id);
        Task<RecruitmentPlanDTO> SaveAsync(RecruitmentPlanDTO model);
        Task SaveAsync(List<SpecialityPlansDTO> plans, string facultyUrl, int year);
    }
}
