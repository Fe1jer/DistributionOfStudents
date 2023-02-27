using DistributionOfStudents.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Data.Models
{
    public class GroupOfSpecialties : Entity
    {
        [Display(Name = "Название")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.Text)]
        public string? Description { get; set; }

        [Display(Name = "Начало")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Окончание")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Год")]
        public int Year { get; set; }

        public bool IsDailyForm { get; set; }
        public bool IsBudget { get; set; }
        public bool IsFullTime { get; set; }

        public bool IsCompleted { get; set; }

        public List<Admission>? Admissions { get; set; }
        public List<Speciality>? Specialities { get; set; }
    }
}