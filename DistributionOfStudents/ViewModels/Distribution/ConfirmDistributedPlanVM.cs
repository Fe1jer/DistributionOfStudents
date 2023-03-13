using DistributionOfStudents.Data.Models;

namespace DistributionOfStudents.ViewModels.Distribution
{
    public class ConfirmDistributedPlanVM
    {
        public ConfirmDistributedPlanVM() { }
        public ConfirmDistributedPlanVM(RecruitmentPlan plan)
        {
            SpecialityName = plan.Speciality.DirectionName ?? plan.Speciality.FullName;
            PlanId = plan.Id;
            PassingScore = plan.PassingScore;
            DistributedStudents = new((plan.EnrolledStudents ?? new()).Select(i => new ConfirmDistributedStudentVM(i.Student)));
        }
        public int PlanId { get; set; }
        public string SpecialityName { get; set; } = String.Empty;
        public int PassingScore { get; set; }

        public List<ConfirmDistributedStudentVM> DistributedStudents { get; set; } = new();
    }
}
