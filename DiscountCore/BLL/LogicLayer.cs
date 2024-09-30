using System.ComponentModel.DataAnnotations;

namespace DiscountCore.BLL
{
    internal class LogicLayer
    {
        public static void ValidateProperty(object value, object instance, string propertyName)
        {
            ValidationContext context = new ValidationContext(instance) { MemberName = propertyName };
            ICollection<ValidationResult> results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateProperty(value, context, results);
            if (!isValid)
            {
                string errorMessage = string.Join("; ", results);
                throw new ValidationException($"{errorMessage}");
                //throw new ValidationException($"Validation failed for {propertyName }: {errorMessage}");
            }
        }
    }
}
