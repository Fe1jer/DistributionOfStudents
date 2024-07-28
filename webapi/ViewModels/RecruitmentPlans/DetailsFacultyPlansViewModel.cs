using System.ComponentModel.DataAnnotations;

namespace webapi.ViewModels.RecruitmentPlans
{
    public class DetailsFacultyPlansViewModel
    {
        [Display(Name = "Факультет")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Факультет")]
        public string? ShortName { get; set; }

        [Display(Name = "Год")]
        public int Year { get; set; }

        [Display(Name = "План набора")]
        public List<SpecialityPlansViewModel> PlansForSpecialities { get; set; } = new();
    }
}
