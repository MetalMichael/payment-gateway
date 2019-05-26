using System;

namespace PaymentGateway.Models
{
    /// <summary>
    /// Limited Credit Card information, to avoid sending data unecessarily
    /// </summary>
    public class MaskedCardDetails
    {
        public MaskedCardDetails(CardDetails details)
        {
            MaskedCardNumber = MaskCardNumber(details.CardNumber);
        }

        public string MaskedCardNumber { get; }

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
