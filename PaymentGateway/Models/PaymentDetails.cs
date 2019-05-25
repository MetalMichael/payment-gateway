using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Models
{
    public class PaymentDetails
    {
        public CardDetails CardDetails { get; set; }
        public TransactionDetails TransactionDetails { get; set; }
    }
}
