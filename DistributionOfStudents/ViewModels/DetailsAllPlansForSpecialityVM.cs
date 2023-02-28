using DistributionOfStudents.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.ViewModels
{
    public class DetailsAllPlansForSpecialityVM
    {
        [Display(Name = "Специальность")]
        public Speciality Speciality { get; set; }

        [Display(Name = "Бюджет")]
        public int DailyFullBudget { get; set; }

        [Display(Name = "Платное")]
        public int DailyFullPaid { get; set; }

        [Display(Name = "Бюджет сокр.срок")]
        public int DailyAbbreviatedBudget { get; set; }

        [Display(Name = "Платное сокр.срок")]
        public int DailyAbbreviatedPaid { get; set; }

        [Display(Name = "Бюджет")]
        public int EveningFullBudget { get; set; }

        [Display(Name = "Платное")]
        public int EveningFullPaid { get; set; }

        [Display(Name = "Бюджет сокр.срок")]
        public int EveningAbbreviatedBudget { get; set; }

        [Display(Name = "Платное сокр.срок")]
        public int EveningAbbreviatedPaid { get; set; }
    }
}
