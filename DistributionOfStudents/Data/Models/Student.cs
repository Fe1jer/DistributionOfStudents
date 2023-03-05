using DistributionOfStudents.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Data.Models
{
    public class Student : Entity
    {
        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string Surname { get; set; }

        [Display(Name = "Отчество")]
        [DataType(DataType.Text)]
        public string Patronymic { get; set; }

        [Display(Name = "Аттестат")]
        public int GPS { get; set; }

        [Display(Name = "Заявки")]
        public List<Admission>? Admissions { get; set; }
    }
}
