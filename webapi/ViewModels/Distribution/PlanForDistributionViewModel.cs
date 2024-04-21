using webapi.Validations;
using webapi.ViewModels.General;

namespace webapi.ViewModels.Distribution
{
    [ValidateDistributedPlan]
    public class PlanForDistributionViewModel : BaseViewModel
    {
        public int PassingScore { get; set; }
        public int Count { get; set; }
        public List<IsDistributedStudentVM> DistributedStudents { get; set; } = new();
    }
}
