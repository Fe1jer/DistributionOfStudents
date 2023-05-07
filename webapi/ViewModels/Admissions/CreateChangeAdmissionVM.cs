using webapi.Data.Models;
using webapi.Validations;
using System.ComponentModel.DataAnnotations;

namespace webapi.ViewModels.Admissions
{
    public class CreateChangeAdmissionVM
    {
        public CreateChangeAdmissionVM() { }
        public CreateChangeAdmissionVM(Admission admission, List<SpecialityPriorityVM> priorities)
        {
            SpecialitiesPriority = priorities;
            StudentScores = admission.StudentScores;
            Student = admission.Student;
            DateOfApplication = admission.DateOfApplication;
            PassportID = admission.PassportID;
            PassportSeries = admission.PassportSeries;
            PassportNumber = admission.PassportNumber;
            Email = admission.Email;
            IsTargeted = admission.IsTargeted;
            IsWithoutEntranceExams = admission.IsWithoutEntranceExams;
            IsOutOfCompetition = admission.IsOutOfCompetition;
        }

        public CreateChangeAdmissionVM(List<StudentScore> scores, List<SpecialityPriorityVM> priorities)
        {
            SpecialitiesPriority = priorities;
            StudentScores = scores;
        }

        public Student Student { get; set; } = new();

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
        public List<StudentScore> StudentScores { get; set; } = new();

        [Display(Name = "Приоритет специальностей (0 - не участвует)")]
        [ValidateSpecialitiesPriority]
        public List<SpecialityPriorityVM> SpecialitiesPriority { get; set; } = new();

    }
}
