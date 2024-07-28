using System.ComponentModel.DataAnnotations;
using webapi.ViewModels.General;

namespace webapi.ViewModels.GroupsOfSpecialities
{
    public class SelectedSpecialityViewModel : BaseViewModel
    {

        [Display(Name = "Специальность")]
        public string FullName { get; set; } = string.Empty;


        [Display(Name = "Состоит ли в группе")]
        public bool IsSelected { get; set; }
    }
}
