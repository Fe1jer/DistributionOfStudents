using webapi.Validations;

namespace webapi.ViewModels.Distribution
{
    [ValidateDistributedPlan]
    public class PlanForDistributionViewModel : BaseViewModel
    {
        public int PlanId { get; set; }
        public int PassingScore { get; set; }
        public int PlanCount { get; set; }
        public List<IsDistributedStudentVM> DistributedStudents { get; set; } = new();
    }
}
