using System.ComponentModel.DataAnnotations;
using webapi.ViewModels.GroupsOfSpecialities;

namespace webapi.Validations
{
    public class ValidateSelectedSpecialities : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            List<SelectedSpecialityViewModel>? dt = (List<SelectedSpecialityViewModel>?)value;

            if (dt != null && dt.Where(i => i.IsSelected == true).Any())
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Выберите специальности");
            }
        }
    }
}
