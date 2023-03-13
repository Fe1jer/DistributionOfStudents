using DistributionOfStudents.Data.Models;
using DistributionOfStudents.ViewModels.GroupsOfSpecialities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DistributionOfStudents.Validations
{
    public class ValidateSelectedSubjects : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            List<IsSelectedSubjectVM>? dt = (List<IsSelectedSubjectVM>?)value;

            if (dt != null && dt.Where(i => i.IsSelected == true).Any())
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Выберите предметы");
            }
        }
    }
}
