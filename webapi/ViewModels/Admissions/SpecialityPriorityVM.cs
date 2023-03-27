using webapi.Data.Models;

namespace webapi.ViewModels.Admissions
{
    public class SpecialityPriorityVM
    {
        public SpecialityPriorityVM() { }
        public SpecialityPriorityVM(RecruitmentPlan plan)
        {
            PlanId = plan.Id;
            NameSpeciality = plan.Speciality.DirectionName ?? plan.Speciality.FullName;
        }

        public int PlanId { get; set; }
        public string NameSpeciality { get; set; } = String.Empty;
        public int Priority { get; set; }
    }
}
