using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels
{
    public class DetailsFacultyRecruitmentPlans
    {
        [Display(Name = "Факультет")]
        public string FacultyFullName { get; set; }

        [Display(Name = "Факультет")]
        public string? FacultyShortName { get; set; }

        [Display(Name = "Год")]
        public int Year { get; set; }
        
        [Display(Name = "План набора")]
        public List<PlansForSpecialityVM> PlansForSpecialities { get; set; }
    }
}
