using PaymentGateway.SharedModels;
using System;

namespace PaymentGateway.Models
{
    /// <summary>
    /// Result of an attempted Payment
    /// </summary>
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

        /// <summary>
        /// Create a new ProcessPaymentResult
        /// </summary>
        /// <param name="paymentId">ID of the Payment</param>
        /// <param name="response">Response from the Bank</param>
        public ProcessPaymentResult(Guid paymentId, PaymentResponse response)
        {
            PaymentId = paymentId;
            Success = response.Successful;
            TransactionId = response.TransactionId;
        }
    }
}