using webapi.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class GroupOfSpecialties : Entity
    {
        [Display(Name = "Название")]
        [DataType(DataType.Text)]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Описание")]
        [DataType(DataType.Text)]
        public string? Description { get; set; }

        [Display(Name = "Начало")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Окончание")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        public bool IsCompleted { get; set; }

        public FormOfEducation FormOfEducation { get; set; } = new();

        public List<Subject>? Subjects { get; set; }
        public List<Admission>? Admissions { get; set; }
        public List<Speciality>? Specialities { get; set; }
    }
}