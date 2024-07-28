using webapi.ViewModels.General;

namespace webapi.ViewModels.Admissions
{
    public class StudentScoreViewModel : BaseViewModel
    {
        public SubjectViewModel Subject { get; set; } = null!;
        public Guid SubjectId { get; set; }
        public int Score { get; set; }
    }
}
