using DistributionOfStudents.Data.Models;
using DistributionOfStudents.Validations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DistributionOfStudents.ViewModels
{
    public class CreateChangeFacultyVM
    {
        public Faculty Faculty { get; set; }

        [Display(Name = "Изображение")]
        [ValidateImg]
        public IFormFile? Img { get; set; }
    }
}
