using System.ComponentModel.DataAnnotations;
using webapi.ViewModels.General;

namespace webapi.ViewModels.GroupsOfSpecialities
{
    public class GroupOfSpecialitiesViewModel : BaseViewModel
    {
        [Display(Name = "Название")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        [DataType(DataType.Text)]
        public string? Description { get; set; }

        [Display(Name = "Начало")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Окончание")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        public bool IsCompleted { get; set; }

        public FormOfEducationViewModel FormOfEducation { get; set; } = null!;
    }
}
