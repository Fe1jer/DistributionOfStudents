using System.ComponentModel.DataAnnotations;
using webapi.Data.Models;
using webapi.Validations;

namespace webapi.ViewModels.Users
{
    public class CreateUserVM
    {
        public CreateUserVM() { }

        [Display(Name = "Имя пользователя")]
        [DataType(DataType.Text)]
        public string UserName { get; set; } = String.Empty;

        [DataType(DataType.Text)]
        public string Password { get; set; } = String.Empty;

        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string Surname { get; set; } = String.Empty;

        [Display(Name = "Отчество")]
        [DataType(DataType.Text)]
        public string Patronymic { get; set; } = String.Empty;

        [Display(Name = "Изображение")]
        [ValidateImg]
        public IFormFile? FileImg { get; set; }
    }

    public class ChangeUserVM
    {
        public ChangeUserVM() { }
        public ChangeUserVM(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            Patronymic = user.Patronymic;
        }
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string Surname { get; set; } = String.Empty;

        [Display(Name = "Отчество")]
        [DataType(DataType.Text)]
        public string Patronymic { get; set; } = String.Empty;

        [Display(Name = "Изображение")]
        [ValidateImg]
        public IFormFile? FileImg { get; set; }
    }
}
