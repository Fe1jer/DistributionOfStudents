using webapi.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace webapi.Data.Models
{
    public class Faculty : Entity
    {
        [Display(Name = "Полное название")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите название")]
        public string FullName { get; set; } = String.Empty;

        [Display(Name = "Сокращённое название")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите название")]
        public string ShortName { get; set; } = String.Empty;

        [Display(Name = "Изображение")]
        [DataType(DataType.ImageUrl)]
        public string Img { get; set; } = "\\img\\Faculties\\Default.jpg";

        public List<Speciality>? Specialities { get; set; }
    }
}