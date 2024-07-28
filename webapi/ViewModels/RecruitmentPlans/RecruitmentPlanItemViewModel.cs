using webapi.ViewModels.General;
using webapi.ViewModels.Students;

namespace webapi.ViewModels.RecruitmentPlans
{
    public class RecruitmentPlanItemViewModel : BaseViewModel
    {
        public string SpecialityName { get; set; }
        public int Count { get; set; }
        public int Target { get; set; }
        public int TargetPassingScore { get; set; }
        public int PassingScore { get; set; }
        public List<EnrolledStudentViewModel>? EnrolledStudents { get; set; }
    }
}
