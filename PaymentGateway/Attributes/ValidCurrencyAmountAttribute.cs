using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Attributes
{
    /// <summary>
    /// Custom Validator to check that the amount of currency is valid
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ValidCurrencyAmountAttribute : ValidationAttribute
    {
        private decimal _min;
        private decimal? _max;
        private int _decimalPlaces;

        /// <summary>
        /// Validate the input to be an amount of money.
        /// </summary>
        /// <param name="minimum">Minimum amount of the currency</param>
        /// <param name="maximum">Maximum amount of the currency</param>
        /// <param name="numberDecimals">Maximum number of decimal places</param>
        public ValidCurrencyAmountAttribute(string minimum = "0.01", string maximum = null, int numberDecimals = 2)
        {
            // Can't use decimals on attribute constructors
            _min = decimal.Parse(minimum);
            if (maximum != null)
            {
                _max = decimal.Parse(maximum);
            }
            _decimalPlaces = numberDecimals;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} is not a valid amount of currency for this use";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currency = value as decimal?;
            if (!currency.HasValue)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            var roundedCurrency = Math.Round(currency.Value, _decimalPlaces);            
            if(roundedCurrency != currency.Value)
            {
                return new ValidationResult($"{validationContext.DisplayName} uses more than {_decimalPlaces} decimal places");
            }

            if (currency.Value < _min)
            {
                return new ValidationResult($"{validationContext.DisplayName} is less than the minimum allowed value: {_min}");
            }

            if (_max.HasValue && currency.Value > _max.Value)
            {
                return new ValidationResult($"{validationContext.DisplayName} is more than the maximum allowed value: {_max}");
            }

            return ValidationResult.Success;
        }
    }
}
