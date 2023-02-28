using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.ViewModels
{
    public class DetailsAllPlansForSpecialityVM
    {
        [Display(Name = "План набора")]
        public List<PlansForSpecialityVM> PlansForSpecialities { get; set; }

        [Display(Name = "Год")]
        public int Year { get; set; }
    }
}
