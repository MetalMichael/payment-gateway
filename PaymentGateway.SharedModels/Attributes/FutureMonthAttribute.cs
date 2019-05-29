using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.SharedModels.Attributes
{
    /// <summary>
    /// Custom validator to check that the month of a given date is in the future
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FutureMonthAttribute : ValidationAttribute
    {
        protected bool _allowNull;

        public FutureMonthAttribute(bool allowNull = false)
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
                // Would probably be best to leave it to the bank, and just ensure that the date is a valid date
                var now = DateTime.UtcNow.Date;
                if (date.Value.Date.Year >= now.Year && date.Value.Date.Month >= now.Month)
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
