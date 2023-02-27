using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Data.Models
{
    public class User : IdentityUser
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

        [Display(Name = "Изображение")]
        public string Img { get; set; }
    }
}
