using System;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
    public class CouchbasePaymentRequestStore : IPaymentRequestStore
    {
        private const string TRANSACTION_BUCKET = "Transactions";

        private readonly IBucket _bucket;

        public CouchbasePaymentRequestStore(IBucketProvider provider)
        {
            _bucket = provider.GetBucket(TRANSACTION_BUCKET);
        }

        public async Task LogPaymentRequest(PaymentRequestLog request)
        {
            var document = new Document<PaymentRequestLog>()
            {
                Id = request.Id.ToString(),
                Content = request
            };

            var result = await _bucket.UpsertAsync(document);
            if (!result.Success)
                throw new CouchbaseResponseException("Could not Log Payment");

        }

        public async Task<PaymentRequestLog> FindPaymentRequest(Guid id)
        {
            var query = await _bucket.GetAsync<PaymentRequestLog>(id.ToString());
            if (query.Success)
                return query.Value;

            if (query.Status == Couchbase.IO.ResponseStatus.KeyNotFound)
                throw new RecordNotFoundException();

            throw new CouchbaseResponseException("Could not Find Payment Request");
        }
    }
}
