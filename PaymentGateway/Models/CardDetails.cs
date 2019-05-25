using PaymentGateway.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    /// <summary>
    /// Credit Card information
    /// </summary>
    public class CardDetails
    {
        /// <summary>
        /// Name of the Cardholder
        /// </summary>
        public string CardholderName { get; set; }

        /// <summary>
        /// 16 digit card number
        /// </summary>
        [StringLength(16)]
        public string CardNumber { get; set; }

        /// <summary>
        /// Expiry date of the card
        /// </summary>
        [FutureDate]
        public DateTime Expires { get; set; }

        /// <summary>
        /// Valid from date of the card (optional)
        /// </summary>
        [PastDate(true)]
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Card Security Code (a.k.a. CVV, CVD, CVC, etc.)
        /// </summary>
        public ushort CSC { get; set; }
    }
}