using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.ViewModels.GroupsOfSpecialities
{
    public class DetailsGroupOfSpecialitiesVM
    {
        public DetailsGroupOfSpecialitiesVM() { }
        public DetailsGroupOfSpecialitiesVM(GroupOfSpecialties group, List<RecruitmentPlan> plans, int year, float competition = 0)
        {
            GroupOfSpecialties = group;
            RecruitmentPlans = plans;
            Year = year;
            Competition = (float)Math.Round(competition, 2);
        }

        [Display(Name = "год")]
        public int? Year { get; set; }

        [Display(Name = "Конкурс")]
        public float Competition { get; set; }

        public GroupOfSpecialties GroupOfSpecialties { get; set; } = new();

        [Display(Name = "Специальности")]
        public List<RecruitmentPlan> RecruitmentPlans { get; set; } = new();
    }
}
