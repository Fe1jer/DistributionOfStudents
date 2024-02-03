using System.ComponentModel.DataAnnotations;
using webapi.Data.Models;

namespace webapi.ViewModels.GroupsOfSpecialities
{
    public class IsSelectedSubjectVM
    {
        public int Id { get; set; }

        [Display(Name = "Предмет")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Состоит ли в группе")]
        public bool IsSelected { get; set; }
    }
}
