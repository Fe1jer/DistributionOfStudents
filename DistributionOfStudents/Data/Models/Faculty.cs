using DistributionOfStudents.Data.AbstractClasses;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Data.Models
{
    public class Faculty : Entity
    {
        [Display(Name = "Полное название")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите название")]
        public string FullName { get; set; }

        [Display(Name = "Сокращённое название")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите название")]
        public string ShortName { get; set; }

        [Display(Name = "Изображение")]
        [DataType(DataType.ImageUrl)]
        public string? Img { get; set; }

        public List<Speciality>? Specialities { get; set; }
    }
}