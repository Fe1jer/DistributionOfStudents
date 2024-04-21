using System.ComponentModel.DataAnnotations;
using webapi.ViewModels.General;

namespace webapi.ViewModels.GroupsOfSpecialities
{
    public class IsSelectedSubjectVM : BaseViewModel
    {

        [Display(Name = "Предмет")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Состоит ли в группе")]
        public bool IsSelected { get; set; }
    }
}
