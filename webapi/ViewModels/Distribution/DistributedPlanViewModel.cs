using webapi.ViewModels.General;
using webapi.ViewModels.Specialities;
using webapi.ViewModels.Students;

namespace webapi.ViewModels.Distribution
{
    public class DistributedPlanViewModel : BaseViewModel
    {
        public SpecialityViewModel Speciality { get; set; } = null!;
        public int Count { get; set; }
        public int Target { get; set; }
        public int TargetPassingScore { get; set; }
        public int PassingScore { get; set; }
        public List<EnrolledStudentViewModel>? EnrolledStudents { get; set; }
    }
}
