using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.ViewModels
{
    public class DetailsFacultyVM
    {
        [Display(Name = "Факультет")]
        public Faculty Faculty { get; set; }

        public DetailsAllPlansForSpecialityVM AllPlansForSpecialities { get; set; }

        [Display(Name = "Группы")]
        public List<DetailsGroupOfSpecialitiesVM> GroupsOfSpecialties { get;set; }
    }
}
