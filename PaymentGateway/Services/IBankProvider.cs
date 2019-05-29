using PaymentGateway.SharedModels;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public interface IBankProvider
    {
        /// <summary>
        /// Verify the validity of a card's details
        /// </summary>
        /// <param name="cardDetails">Card details to verify</param>
        /// <returns>Validity of the Card Details</returns>
        Task<bool> ValidateCardDetailsAsync(CardDetails cardDetails);

        /// <summary>
        /// Attempt to process a payment (Card Details and Transaction)
        /// </summary>
        /// <param name="cardDetails">Card details to use for the payment</param>
        /// <param name="transaction">Currency and amount to process</param>
        /// <returns>Model containing the result of the Payment and, if applicable, Transaction ID</returns>
        Task<PaymentResponse> ProcessPaymentAsync(CardDetails cardDetails, TransactionDetails transaction);
    }
}
