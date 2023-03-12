using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Validations;
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

        [ValidateSelectedSpecialities]
        [Display(Name = "Специальности, составляющие общий конкурс")]
        public List<IsSelectedSpecialityInGroupVM> SelectedSpecialities { get; set; }

        [ValidateSelectedSubjects]
        [Display(Name = "Предметы, по которым нужны сертификаты")]
        public List<IsSelectedSubjectVM> SelectedSubjects { get; set; }
    }
}
