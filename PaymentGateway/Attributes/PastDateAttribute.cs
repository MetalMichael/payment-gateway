using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Attributes
{
    /// <summary>
    /// Custom Date validator to check that the date
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PastDateAttribute : ValidationAttribute
    {
        protected bool _allowNull;

        public PastDateAttribute(bool allowNull = false)
        {
            _allowNull = allowNull;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} Date must be set to a date in the past";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = value as DateTime?;
            if (date.HasValue)
            {
                // TODO: Edge cases. Should be using local time depending on location of card issue?
                if (date.Value.Date <= DateTime.UtcNow.Date)
                {
                    return ValidationResult.Success;
                }
            }
            else if (_allowNull)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
