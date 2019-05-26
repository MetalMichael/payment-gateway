using System;

namespace PaymentGateway.Models
{
    public class ProcessPaymentResult
    {
        /// <summary>
        /// ID of the payment request
        /// </summary>
        public Guid PaymentId { get; }

        /// <summary>
        /// Whether the payment was successful
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// The ID of the transaction from the bank.
        /// May be null if the request failed
        /// </summary
        public Guid? TransactionId { get; }

        public ProcessPaymentResult(Guid paymentId, PaymentResponse response)
        {
            PaymentId = paymentId;
            Success = response.Successful;
            TransactionId = response.TransactionId;
        }
    }
}