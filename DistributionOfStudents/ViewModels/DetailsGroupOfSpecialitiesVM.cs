using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels
{
    public class DetailsGroupOfSpecialitiesVM
    {
        public GroupOfSpecialties GroupOfSpecialties { get; set; }

        [Display(Name = "Специальность")]
        public List<RecruitmentPlan> RecruitmentPlans { get; set; }
    }
}
