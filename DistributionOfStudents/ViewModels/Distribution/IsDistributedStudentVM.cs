using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.ViewModels.Distribution
{
    public class IsDistributedStudentVM
    {
        [Display(Name = "Выбран для распределения в на специальность")]
        public bool IsDistributed { get; set; }

        [Display(Name = "Баллы по ЦТ(ЦЭ)")]
        public List<StudentScore> StudentScores { get; set; } = new();

        [Display(Name = "ФИО")]
        public Student Student { get; set; } = new();

        public int Score
        {
            get
            {
                return StudentScores.Sum(i => i.Score) + Student.GPS;
            }
        }
    }
}
