using webapi.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class RecruitmentPlan : Entity
    {
        [Display(Name = "Специальность (направление специальности)")]
        public Speciality Speciality { get; set; } = new();

        [Display(Name = "План приема")]
        public int Count { get; set; }

        [Display(Name = "Проходной балл")]
        public int PassingScore { get; set; }

        public List<EnrolledStudent>? EnrolledStudents { get; set; }
        public FormOfEducation FormOfEducation { get; set; } = new();
    }
}