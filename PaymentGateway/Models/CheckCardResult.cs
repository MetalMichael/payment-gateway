namespace PaymentGateway.Models
{
    /// <summary>
    /// The result of checking a Credit Card's validity
    /// </summary>
    public class CheckCardResult
    {
        /// <summary>
        /// Create a new CheckCardResult
        /// </summary>
        /// <param name="valid">Card validity</param>
        public CheckCardResult(bool valid)
        {
            Valid = valid;
        }

        /// <summary>
        /// Whether the Credit Card is valid
        /// </summary>
        public bool Valid { get; }
    }
}
