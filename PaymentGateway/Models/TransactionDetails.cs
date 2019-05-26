namespace PaymentGateway.Models
{
    /// <summary>
    /// Information required to make a transaction request
    /// </summary>
    public class TransactionDetails
    {
        /// <summary>
        /// The transaction currency. Should be an ISO 4217 3 letter currency code
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The amount of currency
        /// </summary>
        public double Amount { get; set; }
    }
}
