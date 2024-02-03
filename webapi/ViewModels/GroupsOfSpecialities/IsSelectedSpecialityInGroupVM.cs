using System.ComponentModel.DataAnnotations;
using webapi.Data.Models;

namespace webapi.ViewModels.GroupsOfSpecialities
{
    public class IsSelectedSpecialityInGroupVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Специальность")]
        public string FulName { get; set; } = string.Empty;


        [Display(Name = "Состоит ли в группе")]
        public bool IsSelected { get; set; }
    }
}
