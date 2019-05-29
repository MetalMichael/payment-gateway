namespace PaymentGateway.SharedModels
{
    /// <summary>
    /// Model containing details needed to make a Payment
    /// </summary>
    public class PaymentDetails
    {
        /// <summary>
        /// Credit Card Details
        /// </summary>
        public CardDetails CardDetails { get; set; }

        /// <summary>
        /// Transaction Details
        /// </summary>
        public TransactionDetails TransactionDetails { get; set; }
    }
}
