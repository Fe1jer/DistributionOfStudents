using webapi.ViewModels.Distribution;
using System.ComponentModel.DataAnnotations;

namespace webapi.Validations
{
    public class ValidateDistributedPlan : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            PlanForDistributionVM? dt = (PlanForDistributionVM?)value;
            if (dt != null)
            {
                if (dt.DistributedStudents.Count > dt.Count && dt.DistributedStudents.Where(i => i.IsDistributed).Count() != dt.Count)
                {
                    return new ValidationResult("Выберите " + dt.Count + " абитуриентов");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            return ValidationResult.Success;
        }
    }
}
