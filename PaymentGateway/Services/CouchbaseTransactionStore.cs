using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
    public class CouchbaseTransactionStore : ITransactionStore
    {
        private const string TRANSACTION_BUCKET = "Transactions";

        private Cluster GetCluster()
        {
            var cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri("http://127.0.0.1") }
            });

            var authenticator = new PasswordAuthenticator("Administrator", "password");
            cluster.Authenticate(authenticator);
            return cluster;
        }

        public async Task LogPaymentRequest(PaymentRequestLog request)
        {
            using (var cluster = GetCluster())
            using (var bucket = cluster.OpenBucket(TRANSACTION_BUCKET))
            {
                var document = new Document<PaymentRequestLog>()
                {
                    Id = request.Id.ToString(),
                    Content = request
                };

                await bucket.UpsertAsync(document);
            }
        }

        public async Task<PaymentRequestLog> FindPaymentRequest(Guid id)
        {
            using (var cluster = GetCluster())
            using (var bucket = cluster.OpenBucket(TRANSACTION_BUCKET))
            {
                var query = await bucket.GetAsync<PaymentRequestLog>(id.ToString());
                if (query.Success)
                    return query.Value;

                throw new RecordNotFoundException();
            }
        }
    }
}
