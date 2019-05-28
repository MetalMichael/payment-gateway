namespace PaymentGateway.SharedModels
{
    public class PaymentDetails
    {
        public CardDetails CardDetails { get; set; }
        public TransactionDetails TransactionDetails { get; set; }
    }
}
