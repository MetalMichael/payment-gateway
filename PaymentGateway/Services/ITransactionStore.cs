using System;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
    public interface ITransactionStore
    {
        Task LogPaymentRequest(Guid id, MaskedCardDetails maskedDetails, TransactionDetails transactionDetails, PaymentResponse response);
    }
}
