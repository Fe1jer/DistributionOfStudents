using DistributionOfStudents.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Data.Models
{
    public class RecruitmentPlan : Entity
    {
        [Display(Name = "Специальность")]
        public Speciality Speciality { get; set; }

        [Display(Name = "План приема")]
        public int Count { get; set; }

        [Display(Name = "Проходной балл")]
        public int PassingScore { get; set; }

        [Display(Name = "Год")]
        public int Year { get; set; }

        public bool IsDailyForm { get; set; }
        public bool IsBudget { get; set; }
        public bool IsFullTime { get; set; }

        public List<EnrolledStudent>? EnrolledStudents { get; set; }
    }
}