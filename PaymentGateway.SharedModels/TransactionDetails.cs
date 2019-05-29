using PaymentGateway.SharedModels.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.SharedModels
{
    /// <summary>
    /// Information regarding the amount of currency in a transaction
    /// </summary>
    public class TransactionDetails
    {
        /// <summary>
        /// The transaction currency. Should be an ISO 4217 3 letter currency code
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The amount of currency
        /// </summary>
        [Required]
        [ValidCurrencyAmount]
        public decimal Amount { get; set; }
    }
}
