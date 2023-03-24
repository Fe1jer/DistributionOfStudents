using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.ViewModels.GroupsOfSpecialities
{
    public class DetailsGroupOfSpecialitiesVM
    {
        public DetailsGroupOfSpecialitiesVM() { }
        public DetailsGroupOfSpecialitiesVM(GroupOfSpecialties group, List<RecruitmentPlan> plans, float competition = 0)
        {
            GroupOfSpecialties = group;
            RecruitmentPlans = plans;
            Competition = (float)Math.Round(competition, 2);
        }
        public DetailsGroupOfSpecialitiesVM(string groupOfSpecialtiesName, List<RecruitmentPlan> plans, float competition = 0)
        {
            GroupOfSpecialtiesName = groupOfSpecialtiesName;
            RecruitmentPlans = plans;
            Competition = (float)Math.Round(competition, 2);
        }
        public DetailsGroupOfSpecialitiesVM(GroupOfSpecialties group, List<RecruitmentPlan> plans, string? searchValue, float competition = 0) : this(group, plans, competition)
        {
            SearchValue = searchValue;
        }

        public string? SearchValue { get; set; }

        [Display(Name = "Конкурс")]
        public float Competition { get; set; }

        public string GroupOfSpecialtiesName { get; set; } = String.Empty;
        public GroupOfSpecialties GroupOfSpecialties { get; set; } = new();

        [Display(Name = "Специальности")]
        public List<RecruitmentPlan> RecruitmentPlans { get; set; } = new();
    }
}
