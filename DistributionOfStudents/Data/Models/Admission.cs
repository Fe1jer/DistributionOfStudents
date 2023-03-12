using DistributionOfStudents.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Data.Models
{
    public class Admission : Entity
    {
        [Display(Name = "ФИО")]
        public Student Student { get; set; } = new();
        public GroupOfSpecialties GroupOfSpecialties { get; set; } = new();

        [Display(Name = "Подача заявки")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfApplication { get; set; }

        [Display(Name = "Приоритет")]
        public List<SpecialityPriority> SpecialityPriorities { get; set; } = new();

        [Display(Name = "Баллы по ЦТ(ЦЭ)")]
        public List<StudentScore> StudentScores { get; set; } = new();

        public int Score
        {
            get
            {
                return StudentScores.Sum(i => i.Score) + Student.GPS;
            }
        }
    }
}
