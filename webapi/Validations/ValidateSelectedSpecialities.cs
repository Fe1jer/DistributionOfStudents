﻿using webapi.ViewModels.GroupsOfSpecialities;
using System.ComponentModel.DataAnnotations;

namespace webapi.Validations
{
    public class ValidateSelectedSpecialities : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            List<IsSelectedSpecialityInGroupVM>? dt = (List<IsSelectedSpecialityInGroupVM>?)value;

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
