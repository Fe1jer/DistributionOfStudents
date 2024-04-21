using System.ComponentModel.DataAnnotations;
using webapi.Validations;
using webapi.ViewModels.General;

namespace webapi.ViewModels.Faculties
{
    public class FacultyViewModel : BaseViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Img { get; set; } = "\\img\\Faculties\\Default.jpg";

        [Display(Name = "Изображение")]
        [ValidateImg]
        public IFormFile? FileImg { get; set; }
    }
}
