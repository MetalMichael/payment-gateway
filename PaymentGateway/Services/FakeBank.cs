using System;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
    public class FakeBank : IBankProvider
    {
        public Task<PaymentResponse> ProcessPayment(CardDetails cardDetails, TransactionDetails transaction)
        {
            return Task.FromResult(new PaymentResponse
            {
                Successful = true,
                TransactionId = Guid.NewGuid()
            });
        }

        public Task<bool> ValidateCardDetails(CardDetails cardDetails)
        {
            return Task.FromResult(true);
        }
    }
}
