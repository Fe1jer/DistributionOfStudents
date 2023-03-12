using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.ViewModels
{
    public class IsSelectedSpecialityInGroupVM
    {
        [Display(Name = "Специальность")] 
        public string SpecialityName { get; set; } = String.Empty;

        public int SpecialityId { get; set; }

        [Display(Name = "Состоит ли в группе")]
        public bool IsSelected { get; set; }
    }
}
