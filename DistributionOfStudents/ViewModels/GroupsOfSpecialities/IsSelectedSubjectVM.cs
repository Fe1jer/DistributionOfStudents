using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels.GroupsOfSpecialities
{
    public class IsSelectedSubjectVM
    {
        public IsSelectedSubjectVM() { }
        public IsSelectedSubjectVM(Subject subject, bool isSelected)
        {
            SubjectId = subject.Id;
            Subject = subject.Name;
            IsSelected = isSelected;
        }
        [Display(Name = "Предмет")]
        public string Subject { get; set; } = string.Empty;

        public int SubjectId { get; set; }

        [Display(Name = "Состоит ли в группе")]
        public bool IsSelected { get; set; }
    }
}
