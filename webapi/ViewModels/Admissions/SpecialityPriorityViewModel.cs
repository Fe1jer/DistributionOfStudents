using webapi.Data.Models;

namespace webapi.ViewModels.Admissions
{
    public class SpecialityPriorityViewModel
    {
        public int PlanId { get; set; }
        public string NameSpeciality { get; set; } = String.Empty;
        public int Priority { get; set; }
    }
}
