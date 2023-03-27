using webapi.Data.Models;
using webapi.Validations;

namespace webapi.ViewModels.Distribution
{
    [ValidateDistributedPlan]
    public class PlanForDistributionVM
    {
        public PlanForDistributionVM() { }
        public PlanForDistributionVM(RecruitmentPlan plan)
        {
            SpecialityName = plan.Speciality.FullName;
            PlanId = plan.Id;
            Count = plan.Count;
            PassingScore = plan.PassingScore;
        }

        public int PlanId { get; set; }
        public int Count { get; set; }
        public string SpecialityName { get; set; } = String.Empty;
        public int PassingScore { get; set; }

        public List<IsDistributedStudentVM> DistributedStudents { get; set; } = new();
    }
}
