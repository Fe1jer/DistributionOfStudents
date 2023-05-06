using webapi.Data.Models;

namespace webapi.Data.Interfaces.Services
{
    public interface IDistributionService
    {
        public float Competition { get; }
        public bool AreControversialStudents();
        public List<RecruitmentPlan> GetPlansWithEnrolledStudents();
        public List<RecruitmentPlan> GetPlansWithPassingScores();
        public void NotifyEnrolledStudents();
    }
}
