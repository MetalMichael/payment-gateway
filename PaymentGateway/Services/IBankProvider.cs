using PaymentGateway.Models;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public interface IBankProvider
    {
        Task<bool> ValidateCardDetails(CardDetails cardDetails);

        Task<PaymentResponse> ProcessPayment(CardDetails cardDetails, TransactionDetails transaction);
    }
}
