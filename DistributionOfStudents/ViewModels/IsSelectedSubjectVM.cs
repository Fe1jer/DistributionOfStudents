using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels
{
    public class IsSelectedSubjectVM
    {
        [Display(Name = "Предмет")]
        public string Subject { get; set; }

        public int SubjectId { get; set; }

        [Display(Name = "Состоит ли в группе")]
        public bool IsSelected { get; set; }
    }
}
