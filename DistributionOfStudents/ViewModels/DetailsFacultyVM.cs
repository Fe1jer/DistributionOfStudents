using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.ViewModels
{
    public class DetailsFacultyVM
    {
        [Display(Name = "Факультет")]
        public Faculty Faculty { get; set; } = new();

        public DetailsFacultyRecruitmentPlans AllPlansForSpecialities { get; set; } = new();

        [Display(Name = "Группы")]
        public List<DetailsGroupOfSpecialitiesVM> GroupsOfSpecialties { get;set; } = new();
    }
}
