using DistributionOfStudents.ViewModels.Admissions;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Validations
{
    public class ValidateSpecialitiesPriority : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            List<SpecialityPriorityVM>? dt = (List<SpecialityPriorityVM>?)value;
            if (dt != null)
            {
                if (dt.Any(i => i.Priority < 0))
                {
                    return new ValidationResult("Укажите верный порядок");
                }
                if (dt.All(i => i.Priority == 0))
                {
                    return new ValidationResult("Укажите хоть одну специальность");
                }

                int i = 1;
                foreach (SpecialityPriorityVM specialityPriority in dt.Where(i => i.Priority > 0).OrderBy(i => i.Priority))
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
