using webapi.ViewModels.General;

namespace webapi.ViewModels.Students
{
    public class StudentItemViewModel : BaseViewModel
    {
        public string FacultyName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public Guid GroupOfSpecialtiesId { get; set; }
    }
}
