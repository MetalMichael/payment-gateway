using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.SharedModels.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FutureDateAttribute : ValidationAttribute
    {
        protected bool _allowNull;

        public FutureDateAttribute(bool allowNull = false)
        {
            _allowNull = allowNull;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} Date must be set to a date in the future";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = value as DateTime?;
            if (date.HasValue)
            {
                // TODO: Edge cases. Should be using local time depending on location of card issue?
                if (date.Value.Date >= DateTime.UtcNow.Date)
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
