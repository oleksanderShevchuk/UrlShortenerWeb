using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace UrlShortenerWeb.Attributes
{
    public class PositiveIntegerAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value is int intValue)
                {
                    if (intValue > 0)
                    {
                        return ValidationResult.Success;
                    }
                }
            }
            return new ValidationResult(ErrorMessage ?? "The field must be a positive integer.");
        }
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
                ErrorMessageString, name);
        }
    }
}
