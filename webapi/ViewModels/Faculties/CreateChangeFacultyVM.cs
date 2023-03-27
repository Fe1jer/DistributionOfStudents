using webapi.Data.Models;
using webapi.Validations;
using System.ComponentModel.DataAnnotations;

namespace webapi.ViewModels.Faculties
{
    public class CreateChangeFacultyVM
    {
        public CreateChangeFacultyVM() { }
        public CreateChangeFacultyVM(Faculty faculty)
        {
            Faculty = faculty;
        }

        public Faculty Faculty { get; set; } = new();

        [Display(Name = "Изображение")]
        [ValidateImg]
        public IFormFile? Img { get; set; }
    }
}
