using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Models
{
    /// <summary>
    /// Response from a Bank after an attempted payment
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// Whether the transaction succeeded
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// ID of the transaction, if successful
        /// </summary>
        public Guid TransactionId { get; set; }
    }
}
