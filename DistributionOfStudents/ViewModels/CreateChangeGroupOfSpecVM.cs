using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels
{
    public class CreateChangeGroupOfSpecVM
    {
        [Display(Name = "Факультет")]
        public string FacultyShortName { get; set; }

        [Display(Name = "Группа")]
        public GroupOfSpecialties Group { get; set; }

        [Display(Name = "Специальности, входящие в группу")]
        public List<IsSelectedSpecialityInGroupVM> SelectedSpecialities { get; set; }
    }
}
