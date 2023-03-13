using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels.RecruitmentPlans
{
    public class DetailsFacultyRecruitmentPlans
    {
        public DetailsFacultyRecruitmentPlans() { }
        public DetailsFacultyRecruitmentPlans(Faculty faculty, List<PlansForSpecialityVM> plans, int year)
        {
            FacultyFullName = faculty.FullName;
            PlansForSpecialities = plans;
            FacultyShortName = faculty.ShortName;
            Year = year;
        }

        [Display(Name = "Факультет")]
        public string FacultyFullName { get; set; } = string.Empty;

        [Display(Name = "Факультет")]
        public string? FacultyShortName { get; set; }

        [Display(Name = "Год")]
        public int Year { get; set; }

        [Display(Name = "План набора")]
        public List<PlansForSpecialityVM> PlansForSpecialities { get; set; } = new();
    }
}
