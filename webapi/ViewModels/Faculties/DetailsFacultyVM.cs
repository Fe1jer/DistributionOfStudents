using webapi.Data.Models;
using webapi.ViewModels.GroupsOfSpecialities;
using webapi.ViewModels.RecruitmentPlans;
using System.ComponentModel.DataAnnotations;

namespace webapi.ViewModels.Faculties
{
    public class DetailsFacultyVM
    {
        public DetailsFacultyVM() { }
        public DetailsFacultyVM(Faculty faculty, DetailsFacultyRecruitmentPlans plans, List<DetailsGroupOfSpecialitiesVM> groups)
        {
            Faculty = faculty;
            GroupsOfSpecialties = groups;
            AllPlansForSpecialities = plans;
        }

        [Display(Name = "Факультет")]
        public Faculty Faculty { get; set; } = new();

        public DetailsFacultyRecruitmentPlans AllPlansForSpecialities { get; set; } = new();

        [Display(Name = "Группы")]
        public List<DetailsGroupOfSpecialitiesVM> GroupsOfSpecialties { get; set; } = new();
    }
}
