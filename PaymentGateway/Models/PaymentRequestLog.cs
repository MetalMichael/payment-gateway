using System;
using PaymentGateway.SharedModels;

namespace PaymentGateway.Models
{
    /// <summary>
    /// Record of an attempted transaction
    /// </summary>
    public class PaymentRequestLog
    {
        /// <summary>
        /// ID of the Payment Request (Payment ID)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Masked Credit Card Details
        /// </summary>
        public MaskedCardDetails MaskedCardDetails { get; set; }

        /// <summary>
        /// Details of the Transaction, such as Currency and Amount
        /// </summary>
        public TransactionDetails TransactionDetails { get; set; }

        /// <summary>
        /// Response from the Bank of the attempted Payment
        /// </summary>
        public PaymentResponse PaymentResponse { get; set; }
    }
}
