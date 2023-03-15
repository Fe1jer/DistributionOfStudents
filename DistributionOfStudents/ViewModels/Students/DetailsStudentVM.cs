using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels.Students
{
    public class DetailsStudentVM
    {
        public DetailsStudentVM() { }
        public DetailsStudentVM(string fullName, string facultyName, GroupOfSpecialties group)
        {
            FullName = fullName;
            FacultyName = facultyName;
            Group = group;
        }

        [Display(Name = "ФИО")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Факультет")]
        public string FacultyName { get; set; } = string.Empty;

        [Display(Name = "Форма образования")]
        public GroupOfSpecialties Group { get; set; } = new();
    }
}
