using webapi.ViewModels.General;

namespace webapi.ViewModels.Admissions
{
    public class SpecialityPriorityViewModel : BaseViewModel
    {
        public Guid RecruitmentPlanId { get; set; }
        public string SpecialityName { get; set; } = String.Empty;
        public int Priority { get; set; }
    }
}
