using System;
using System.Threading.Tasks;
using PaymentGateway.SharedModels;

namespace PaymentGateway.Services
{
    /// <summary>
    /// Stub bank to support standalone development
    /// </summary>
    public class FakeBank : IBankProvider
    {
        /// <summary>
        /// Verify the validity of a card's details
        /// </summary>
        /// <param name="cardDetails">Card details to verify</param>
        /// <returns>Validity of the Card Details</returns>
        public Task<bool> ValidateCardDetails(CardDetails cardDetails)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Attempt to process a payment (Card Details and Transaction)
        /// </summary>
        /// <param name="cardDetails">Card details to use for the payment</param>
        /// <param name="transaction">Currency and amount to process</param>
        /// <returns>Model containing the result of the Payment and, if applicable, Transaction ID</returns>
        public Task<PaymentResponse> ProcessPayment(CardDetails cardDetails, TransactionDetails transaction)
        {
            return Task.FromResult(new PaymentResponse
            {
                Successful = true,
                TransactionId = Guid.NewGuid()
            });
        }
    }
}
