using webapi.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class StudentScore : Entity
    {
        public Subject Subject { get; set; } = new();

        [Display(Name = "Баллы")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Введите значение больше 0")]
        public int Score { get; set; }
    }
}