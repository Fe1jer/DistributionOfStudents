using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Validations;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.ViewModels.GroupsOfSpecialities
{
    public class CreateChangeGroupOfSpecVM
    {
        [Display(Name = "Группа")]
        public GroupOfSpecialties Group { get; set; } = new();

        [ValidateSelectedSpecialities]
        [Display(Name = "Специальности, составляющие общий конкурс")]
        public List<IsSelectedSpecialityInGroupVM> SelectedSpecialities { get; set; } = new();

        [ValidateSelectedSubjects]
        [Display(Name = "Предметы, по которым нужны сертификаты")]
        public List<IsSelectedSubjectVM> SelectedSubjects { get; set; } = new();
    }
}
