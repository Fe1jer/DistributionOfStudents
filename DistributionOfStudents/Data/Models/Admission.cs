using DistributionOfStudents.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Data.Models
{
    public class Admission : Entity
    {
        [Display(Name = "ФИО")]
        public Student Student { get; set; }
        public GroupOfSpecialties GroupOfSpecialties { get; set; }

        [Display(Name = "Подача заявки")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfApplication { get; set; }

        [Display(Name = "Приоритет")]
        public List<SpecialityPriority> SpecialityPriorities { get; set; }

        [Display(Name = "Баллы по ЦТ(ЦЭ)")]
        public List<StudentScore> StudentScores { get; set; }

        public int Score
        {
            get
            {
                return StudentScores.Sum(i => i.Score) + Student.GPS;
            }
        }
    }
}
