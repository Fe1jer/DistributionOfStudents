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
                if (dt.DistributedStudents.Count > dt.PlanCount && dt.DistributedStudents.Where(i => i.IsDistributed).Count() != dt.PlanCount)
                {
                    return new ValidationResult("Выберите " + dt.PlanCount + " абитуриентов");
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
