using BLL.DTO.RecruitmentPlans;

namespace BLL.Services.Interfaces
{
    public interface IDistributionService
    {
        public float Competition { get; }
        public bool AreControversialStudents();
        public List<RecruitmentPlanDTO> GetPlansWithEnrolledStudents();
        public List<RecruitmentPlanDTO> GetPlansWithPassingScores();
        public void NotifyEnrolledStudents();
    }
}
