using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels
{
    public class DetailsGroupOfSpecialitiesVM
    {        
        [Display(Name = "год")]
        public int? Year{ get; set; }
        
        [Display(Name = "Конкурс")]
        public float Competition{ get; set; }

        public GroupOfSpecialties GroupOfSpecialties { get; set; } = new();

        [Display(Name = "Специальности")]
        public List<RecruitmentPlan> RecruitmentPlans { get; set; } = new();
    }
}
