﻿using System.ComponentModel.DataAnnotations;

namespace webapi.Validations
{
    public class ValidateImg : ValidationAttribute
    {
        public static readonly string[] mimeTypes = new[] { "image/jpeg", "image/png" };
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            IFormFile? dt = (IFormFile?)value;

            if (dt == null || mimeTypes.Contains(dt.ContentType))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Выберите изображение");
            }
        }
    }
}
