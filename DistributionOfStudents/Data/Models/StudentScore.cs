using DistributionOfStudents.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Data.Models
{
    public class StudentScore : Entity
    {
        public Subject Subject { get; set; }

        [Display(Name = "Баллы")]
        public int Score { get; set; }
    }
}