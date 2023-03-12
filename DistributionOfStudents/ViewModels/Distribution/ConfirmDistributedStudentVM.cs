using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels.Distribution
{
    public class ConfirmDistributedStudentVM
    {
        public int StudentId { get; set; }

        [Display(Name = "ФИО")]
        public string FullName { get; set; }
    }
}
