using System.ComponentModel.DataAnnotations;
using webapi.ViewModels.General;

namespace webapi.ViewModels.Specialities
{
    public class SpecialityViewModel : BaseViewModel
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Введите название")]
        public string FullName { get; set; } = String.Empty;

        [DataType(DataType.Text)]
        public string? ShortName { get; set; }

        [Required(ErrorMessage = "Введите код")]
        public string Code { get; set; } = String.Empty;

        public string? ShortCode { get; set; }

        [DataType(DataType.Text)]
        public string? Description { get; set; }

        [DataType(DataType.Text)]
        public string? DirectionName { get; set; }

        public string? DirectionCode { get; set; }

        [Display(Name = "Специализация")]
        [DataType(DataType.Text)]
        public string? SpecializationName { get; set; }

        [Display(Name = "Код специализации")]
        public string? SpecializationCode { get; set; }

        public bool IsDisabled { get; set; }

        public Guid FacultyId { get; set; }
    }
}
