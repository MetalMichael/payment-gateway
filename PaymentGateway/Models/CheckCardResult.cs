namespace PaymentGateway.Models
{
    public class CheckCardResult
    {
        public CheckCardResult(bool valid)
        {
            Valid = valid;
        }

        public bool Valid { get; }
    }
}
