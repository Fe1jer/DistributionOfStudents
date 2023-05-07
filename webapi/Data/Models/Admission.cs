using webapi.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
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

        [Display(Name = "Идентификационный номер")]
        public string? PassportID { get; set; }

        [Display(Name = "Серия")]
        public string? PassportSeries { get; set; }

        [Display(Name = "Номер")]
        public int? PassportNumber { get; set; }

        [Display(Name = "Целевое")]
        public bool IsTargeted { get; set; }

        [Display(Name = "Без вступительных экзаменов")]
        public bool IsWithoutEntranceExams { get; set; }

        [Display(Name = "Вне конкурса")]
        public bool IsOutOfCompetition { get; set; }

        [Display(Name = "Почта")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

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
