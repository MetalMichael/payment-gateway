using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public interface IBankProvider
    {
        Task<bool> ValidateCardDetails(CardDetails cardDetails);

        Task<PaymentResponse> ProcessPayment(CardDetails cardDetails, TransactionDetails transaction);
    }
}
