using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.ViewModels
{
    public class DetailsFacultyVM
    {
        [Display(Name = "Факультет")]
        public Faculty Faculty { get; set; }

        [Display(Name = "План набора")]
        public List<DetailsAllPlansForSpecialityVM> AllPlansForSpecialities { get; set; }

        [Display(Name = "Группы")]
        public Dictionary<GroupOfSpecialties, List<RecruitmentPlan>> GroupOfSpecialties { get;set; }
    }
}
