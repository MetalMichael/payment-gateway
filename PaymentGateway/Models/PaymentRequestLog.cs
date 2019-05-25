using System;
using System.Collections.Generic;
using System.Linq;
namespace PaymentGateway.Models
{
    public class PaymentRequestLog
    {
        public Guid Id { get; set; }
        public MaskedCardDetails MaskedCardDetails { get; set; }
        public TransactionDetails TransactionDetails { get; set; }
        public PaymentResponse PaymentResponse { get; set; }
    }
}
