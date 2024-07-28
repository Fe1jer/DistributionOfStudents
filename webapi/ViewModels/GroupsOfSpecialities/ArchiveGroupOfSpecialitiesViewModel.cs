using System.ComponentModel.DataAnnotations;
using webapi.ViewModels.RecruitmentPlans;

namespace webapi.ViewModels.GroupsOfSpecialities
{
    public class ArchiveGroupOfSpecialitiesViewModel
    {
        [Display(Name = "Конкурс")]
        public float Competition { get; set; }

        public string Name { get; set; } = String.Empty;


        [Display(Name = "Специальности")]
        public List<RecruitmentPlanViewModel> RecruitmentPlans { get; set; } = new();
    }
}
