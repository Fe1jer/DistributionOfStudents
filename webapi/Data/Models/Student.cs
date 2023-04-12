using webapi.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class Student : Entity
    {
        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите фимилию")]
        public string Surname { get; set; } = String.Empty;

        [Display(Name = "Отчество")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите отчество")]
        public string Patronymic { get; set; } = String.Empty;

        [Display(Name = "Аттестат")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Введите значение больше 0")]
        public int GPS { get; set; }

        [Display(Name = "Заявки")]
        public List<Admission>? Admissions { get; set; }
    }
}
