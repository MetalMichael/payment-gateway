using System;
using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
    /// <summary>
    /// Persistant storage of Payment Requests
    /// </summary>
    public interface IPaymentRequestStore
    {
        /// <summary>
        /// Store a record of a Payment attempt, regardless of its successfulness
        /// </summary>
        /// <param name="request">Payment Request information to store</param>
        Task LogPaymentRequest(PaymentRequestLog request);

        /// <summary>
        /// Retrieve a record of a Payment attempt
        /// </summary>
        /// <param name="paymentId">The Payment Request's ID</param>
        /// <exception cref="RecordNotFoundException">Thrown if no Payment Request found with ID</exception>
        /// <returns>Payment Request Log model</returns>
        Task<PaymentRequestLog> FindPaymentRequest(Guid paymentId);
    }
}
