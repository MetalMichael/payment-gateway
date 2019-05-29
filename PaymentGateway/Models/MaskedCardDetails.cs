using System;
using PaymentGateway.SharedModels;

namespace PaymentGateway.Models
{
    /// <summary>
    /// Limited Credit Card information, to avoid sending data unecessarily
    /// </summary>
    public class MaskedCardDetails
    {
        /// <summary>
        /// Empty constructor, needed for Deserialization
        /// </summary>
        public MaskedCardDetails()
        {
        }

        /// <summary>
        /// Create a new MaskedCardDetails model
        /// </summary>
        /// <param name="details">CardDetails to mask</param>
        public MaskedCardDetails(CardDetails details)
        {
            MaskedCardNumber = MaskCardNumber(details.CardNumber);
            CardholderName = details.CardholderName;
            Expires = details.Expires;
            ValidFrom = details.ValidFrom;
        }

        /// <summary>
        /// Censored Credit Card Number (e.g. ************1234)
        /// </summary>
        public string MaskedCardNumber { get; set; }

        /// <summary>
        /// Name of the Cardholder
        /// </summary>
        public string CardholderName { get; set; }

        /// <summary>
        /// Expiry date of the Credit Card
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// (Optional) Date the Credit Card is valid from
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Hide most of the card digits
        /// E.g. ************1234
        /// </summary>
        /// <param name="cardNumber">16 Digit card number, to mask</param>
        /// <returns>Masked card number</returns>
        private static string MaskCardNumber(string cardNumber)
        {
            if (cardNumber?.Length != 16)
                throw new ArgumentException(nameof(cardNumber));

            return new string('*', 12) + cardNumber.Substring(12);
        }
    }
}
