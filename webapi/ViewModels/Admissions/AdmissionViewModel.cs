using System.ComponentModel.DataAnnotations;
using webapi.Validations;
using webapi.ViewModels.General;
using webapi.ViewModels.Students;

namespace webapi.ViewModels.Admissions
{
    public class AdmissionViewModel : BaseViewModel
    {
        public StudentViewModel Student { get; set; } = null!;

        [Display(Name = "Подача заявки")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfApplication { get; set; }
        public string? PassportID { get; set; }
        public string? PassportSeries { get; set; }
        public int? PassportNumber { get; set; }
        public bool IsTargeted { get; set; }
        public bool IsWithoutEntranceExams { get; set; }
        public bool IsOutOfCompetition { get; set; }
        public string? Email { get; set; }

        [Display(Name = "Баллы по ЦТ(ЦЭ)")]
        public List<StudentScoreViewModel> StudentScores { get; set; } = new();

        [Display(Name = "Приоритет специальностей (0 - не участвует)")]
        [ValidateSpecialityPriorities]
        public List<SpecialityPriorityViewModel> SpecialityPriorities { get; set; } = new();

    }
}
