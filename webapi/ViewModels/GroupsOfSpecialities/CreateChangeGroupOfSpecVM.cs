using webapi.Data.Models;
using webapi.Validations;
using System.ComponentModel.DataAnnotations;

namespace webapi.ViewModels.GroupsOfSpecialities
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
