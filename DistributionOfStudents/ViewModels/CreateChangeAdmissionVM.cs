using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Validations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels
{
    public class CreateChangeAdmissionVM
    {
        public int? Id { get; set; }
        public int GroupId { get; set; }
        public string FacultyName { get; set; }

        public Student Student { get; set; }

        [Display(Name = "Подача заявки")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfApplication { get; set; }

        [Display(Name = "Баллы по ЦТ(ЦЭ)")]
        public List<StudentScore> StudentScores { get; set; }

        [Display(Name = "Приоритет специальностей (0 - не участвует)")]
        [ValidateSpecialitiesPriority]
        public List<SpecialityPriorityVM> SpecialitiesPriority { get; set; }

    }
}
