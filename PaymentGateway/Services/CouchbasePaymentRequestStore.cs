using System;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
    /// <summary>
    /// Persistant Data Store to contain logs of payment requests, and their responses
    /// </summary>
    public class CouchbasePaymentRequestStore : IPaymentRequestStore
    {
        private const string TRANSACTION_BUCKET = "Transactions";

        private readonly IBucketProvider _provider;
        private IBucket _bucket;
        private IBucket Bucket
        {
            get
            {
                if (_bucket == null)
                {
                    _bucket = _provider.GetBucket(TRANSACTION_BUCKET);
                }
                return _bucket;
            }
        }

        /// <summary>
        /// Create a new PaymentRequestStore
        /// </summary>
        /// <param name="provider">Couchbase Bucket Provider</param>
        public CouchbasePaymentRequestStore(IBucketProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Store the record of an attempted payment, and its result
        /// </summary>
        /// <param name="request">Log of the payment request</param>
        public async Task LogPaymentRequest(PaymentRequestLog request)
        {
            var document = new Document<PaymentRequestLog>()
            {
                Id = request.Id.ToString(),
                Content = request
            };

            var result = await Bucket.UpsertAsync(document);
            if (!result.Success)
                throw new CouchbaseResponseException("Could not Log Payment");
        }

        /// <summary>
        /// Retrieve the record of an attempted payment, and its result
        /// </summary>
        /// <param name="id">ID of the attempted payment (Payment ID)</param>
        /// <exception cref="RecordNotFoundException">Record with <paramref name="id"/> cannot be found.</exception>
        /// <returns>Record of an attempted payment</returns>
        public async Task<PaymentRequestLog> FindPaymentRequest(Guid id)
        {
            var query = await Bucket.GetAsync<PaymentRequestLog>(id.ToString());
            if (query.Success)
                return query.Value;

            if (query.Status == Couchbase.IO.ResponseStatus.KeyNotFound)
                throw new RecordNotFoundException();

            throw new CouchbaseResponseException("Could not Find Payment Request");
        }
    }
}
