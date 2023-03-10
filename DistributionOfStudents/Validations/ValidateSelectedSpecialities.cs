using DistributionOfStudents.ViewModels;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Validations
{
    public class ValidateSelectedSpecialities : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<IsSelectedSpecialityInGroupVM> dt = (List<IsSelectedSpecialityInGroupVM>)value;

            if (dt == null || !dt.Where(i => i.IsSelected == true).Any())
            {
                return new ValidationResult("Выберите специальности");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
