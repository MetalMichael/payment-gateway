using System;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
    public interface ITransactionStore
    {
        Task LogPaymentRequest(PaymentRequestLog request);

        Task<PaymentRequestLog> FindPaymentRequest(Guid id);
    }
}
