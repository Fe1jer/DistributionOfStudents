using DistributionOfStudents.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Data.Models
{
    public class FormOfEducation : Entity
    {
        [Display(Name = "Год")]
        public int Year { get; set; }

        public bool IsDailyForm { get; set; }
        public bool IsBudget { get; set; }
        public bool IsFullTime { get; set; }
    }
}
