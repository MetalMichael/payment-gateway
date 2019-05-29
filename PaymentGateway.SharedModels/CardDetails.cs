using PaymentGateway.SharedModels.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.SharedModels
{
    /// <summary>
    /// Credit Card information
    /// </summary>
    public class CardDetails
    {
        /// <summary>
        /// Name of the Cardholder
        /// </summary>
        [Required]
        public string CardholderName { get; set; }

        /// <summary>
        /// 16 digit card number
        /// </summary>
        [Required]
        [StringLength(16, MinimumLength = 16)]
        [RegularExpression("[0-9]+")]
        public string CardNumber { get; set; }

        /// <summary>
        /// Expiry date of the card
        /// </summary>
        [Required]
        [FutureMonth]
        public DateTime Expires { get; set; }

        /// <summary>
        /// Valid from date of the card (optional)
        /// </summary>
        [PastMonth(true)]
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Card Security Code (a.k.a. CVV, CVD, CVC, etc.)
        /// </summary>
        [Required]
        [StringLength(3, MinimumLength = 3)]
        [RegularExpression("[0-9]+")]
        public string CSC { get; set; }
    }
}