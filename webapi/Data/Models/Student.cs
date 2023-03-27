using webapi.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class Student : Entity
    {
        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string Surname { get; set; } = String.Empty;

        [Display(Name = "Отчество")]
        [DataType(DataType.Text)]
        public string Patronymic { get; set; } = String.Empty;

        [Display(Name = "Аттестат")]
        public int GPS { get; set; }

        [Display(Name = "Заявки")]
        public List<Admission>? Admissions { get; set; }
    }
}
