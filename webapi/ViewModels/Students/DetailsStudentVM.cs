using System.ComponentModel.DataAnnotations;
namespace webapi.ViewModels.Students
{
    public class DetailsStudentVM
    {
        [Display(Name = "ФИО")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Факультет")]
        public string FacultyName { get; set; } = string.Empty;

        [Display(Name = "Форма образования")]
        public string GroupName { get; set; } = string.Empty;

        public int GroupId { get; set; }
    }
}
