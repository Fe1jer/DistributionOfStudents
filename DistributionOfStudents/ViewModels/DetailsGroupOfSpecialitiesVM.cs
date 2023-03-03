using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels
{
    public class DetailsGroupOfSpecialitiesVM
    {
        [Display(Name = "Факультет")]
        public string FacultyShortName { get; set; }
        
        [Display(Name = "год")]
        public int? Year{ get; set; }

        public GroupOfSpecialties GroupOfSpecialties { get; set; }

        [Display(Name = "Специальность")]
        public List<RecruitmentPlan> RecruitmentPlans { get; set; }
    }
}
