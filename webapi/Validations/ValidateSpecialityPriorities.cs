using System.ComponentModel.DataAnnotations;
using webapi.ViewModels.Admissions;

namespace webapi.Validations
{
    public class ValidateSpecialityPriorities : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            List<SpecialityPriorityViewModel>? dt = (List<SpecialityPriorityViewModel>?)value;
            if (dt != null)
            {
                if (dt.Exists(i => i.Priority < 0))
                {
                    return new ValidationResult("Укажите верный порядок");
                }
                if (dt.All(i => i.Priority == 0))
                {
                    return new ValidationResult("Укажите хоть одну специальность");
                }

                int i = 1;
                foreach (SpecialityPriorityViewModel specialityPriority in dt.Where(i => i.Priority > 0).OrderBy(i => i.Priority))
                {
                    if (specialityPriority.Priority != i)
                    {
                        return new ValidationResult("Укажите верный порядок");
                    }
                    i++;
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Не настроены специальности");
        }
    }
}
