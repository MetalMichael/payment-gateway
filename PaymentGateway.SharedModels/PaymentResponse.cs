using System;

namespace PaymentGateway.SharedModels
{
    /// <summary>
    /// Response from a Bank after an attempted payment
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// Whether the transaction succeeded
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// ID of the transaction, if successful
        /// </summary>
        public Guid TransactionId { get; set; }
    }
}
