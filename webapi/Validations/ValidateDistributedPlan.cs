using System.ComponentModel.DataAnnotations;
using webapi.ViewModels.Distribution;

namespace webapi.Validations
{
    public class ValidateDistributedPlan : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            PlanForDistributionViewModel? dt = (PlanForDistributionViewModel?)value;
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
