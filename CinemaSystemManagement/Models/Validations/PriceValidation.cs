using System.ComponentModel.DataAnnotations;

namespace CinemaSystemManagement.Models.Validations
{
    
        public class PriceValidation : ValidationAttribute
        {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) return new ValidationResult("Price required");

            decimal price = (decimal)value;

            if (price < 50)
                return new ValidationResult("Price must be at least 50");

            return ValidationResult.Success;
        }
    }
    }

