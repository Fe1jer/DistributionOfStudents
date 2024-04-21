using webapi.ViewModels.Specialities;

namespace webapi.ViewModels.RecruitmentPlans
{
    public class RecruitmentPlanArchiveViewModel : RecruitmentPlanViewModel
    {
        public SpecialityViewModel Speciality { get; set; } = null!;
    }
}
