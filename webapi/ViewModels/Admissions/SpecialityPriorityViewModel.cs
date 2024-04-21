using webapi.ViewModels.General;

namespace webapi.ViewModels.Admissions
{
    public class SpecialityPriorityViewModel : BaseViewModel
    {
        public string SpecialityName { get; set; } = String.Empty;
        public int Priority { get; set; }
    }
}
