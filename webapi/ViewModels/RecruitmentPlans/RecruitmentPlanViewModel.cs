using System.ComponentModel.DataAnnotations;
using webapi.ViewModels.General;

namespace webapi.ViewModels.RecruitmentPlans
{
    public class RecruitmentPlanViewModel : BaseViewModel
    {
        [Display(Name = "План приема")]
        public int Count { get; set; }

        [Display(Name = "Целевое")]
        public int Target { get; set; }

        public int TargetPassingScore { get; set; }

        public int PassingScore { get; set; }
    }
}
