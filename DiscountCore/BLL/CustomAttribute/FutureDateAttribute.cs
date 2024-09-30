using System.ComponentModel.DataAnnotations;

namespace DiscountCore.BLL.CustomAttribute
{
    internal class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue && dateValue < DateTime.Now)
                return new ValidationResult(base.ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
