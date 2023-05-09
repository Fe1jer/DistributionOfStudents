using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace webapi.ViewModels.RecruitmentPlans
{
    public class ChangeRecruitmentPlanItemVM
    {
        public int PlanId { get; set; }

        [Display(Name = "План приема")]
        public int Count { get; set; }

        [Display(Name = "Целевое")]
        public int Target { get; set; }
    }
}
