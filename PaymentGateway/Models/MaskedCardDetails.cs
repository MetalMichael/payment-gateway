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

        public MaskedCardDetails(CardDetails details)
        {
            MaskedCardNumber = MaskCardNumber(details.CardNumber);
            CardholderName = details.CardholderName;
            Expires = details.Expires;
            ValidFrom = details.ValidFrom;
        }

        public string MaskedCardNumber { get; set; }
        public string CardholderName { get; set; }
        public DateTime Expires { get; set; }
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
