using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Validations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels.Faculties
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
