using webapi.Data.Models;
using webapi.Validations;
using System.ComponentModel.DataAnnotations;

namespace webapi.ViewModels.Admissions
{
    public class CreateChangeAdmissionVM
    {
        public CreateChangeAdmissionVM() { }
        public CreateChangeAdmissionVM(int id, Admission admission, List<SpecialityPriorityVM> priorities)
        {
            Id = id;
            SpecialitiesPriority = priorities;
            StudentScores = admission.StudentScores;
            Student = admission.Student;
            DateOfApplication = admission.DateOfApplication;
        }

        public CreateChangeAdmissionVM(List<StudentScore> scores, List<SpecialityPriorityVM> priorities)
        {
            SpecialitiesPriority = priorities;
            StudentScores = scores;
        }

        public int? Id { get; set; }

        public Student Student { get; set; } = new();

        [Display(Name = "Подача заявки")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfApplication { get; set; }

        [Display(Name = "Баллы по ЦТ(ЦЭ)")]
        public List<StudentScore> StudentScores { get; set; } = new();

        [Display(Name = "Приоритет специальностей (0 - не участвует)")]
        [ValidateSpecialitiesPriority]
        public List<SpecialityPriorityVM> SpecialitiesPriority { get; set; } = new();

    }
}
