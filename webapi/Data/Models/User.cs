using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using webapi.Data.AbstractClasses;

namespace webapi.Data.Models
{
    public class User : Entity
    {
        [JsonIgnore]
        [Display(Name = "Имя пользователя")]
        [DataType(DataType.Text)]
        public string UserName { get; set; } = String.Empty;

        [JsonIgnore]
        public string PasswordHash { get; set; } = String.Empty;

        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string Surname { get; set; } = String.Empty;

        [Display(Name = "Отчество")]
        [DataType(DataType.Text)]
        public string Patronymic { get; set; } = String.Empty;

        public string Role { get; set; } = String.Empty;

        [Display(Name = "Изображение")]
        public string Img { get; set; } = String.Empty;
    }
}
