using DistributionOfStudents.Validations;

namespace DistributionOfStudents.ViewModels.Distribution
{
    [ValidateDistributedPlan]
    public class PlanForDistributionVM
    {
        public int PlanId { get; set; }
        public int Count { get; set; }
        public string SpecialityName { get; set; }
        public int PassingScore { get; set; }

        public List<IsDistributedStudentVM> DistributedStudents { get; set; }
    }
}
