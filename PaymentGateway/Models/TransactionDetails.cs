using PaymentGateway.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    /// <summary>
    /// Information required to make a transaction request
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
