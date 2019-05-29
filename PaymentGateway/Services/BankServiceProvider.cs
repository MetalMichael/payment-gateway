using Microsoft.Extensions.Logging;
using PaymentGateway.Services.Clients;
using PaymentGateway.SharedModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using CardDetails = PaymentGateway.SharedModels.CardDetails;
using PaymentResponse = PaymentGateway.SharedModels.PaymentResponse;
using TransactionDetails = PaymentGateway.SharedModels.TransactionDetails;

namespace PaymentGateway.Services
{
    public class BankServiceProvider : IBankProvider
    {
        private ILogger<BankServiceProvider> _log;
        private BankClient _client = new BankClient(Environment.GetEnvironmentVariable("BANK_URL"), new HttpClient());

        public BankServiceProvider(ILogger<BankServiceProvider> logger)
        {
            _log = logger;
        }

        public async Task<PaymentResponse> ProcessPayment(CardDetails cardDetails, TransactionDetails transaction)
        {
            try
            {
                var response = await _client.PayAsync(new Clients.PaymentDetails
                {
                    CardDetails = new Clients.CardDetails
                    {
                        CardholderName = cardDetails.CardholderName,
                        CardNumber = cardDetails.CardNumber,
                        Csc = cardDetails.CSC,
                        Expires = cardDetails.Expires,
                        ValidFrom = cardDetails.ValidFrom
                    },
                    TransactionDetails = new Clients.TransactionDetails
                    {
                        Amount = (double)transaction.Amount,
                        Currency = transaction.Currency
                    }
                });
                return new PaymentResponse
                {
                    Successful = response.Successful,
                    TransactionId = response.TransactionId
                };
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error in bank Processing Payment");
                throw;
            }
        }

        public async Task<bool> ValidateCardDetails(CardDetails cardDetails)
        {
            try
            {
                var response = await _client.CheckAsync(new Clients.CardDetails
                {
                    CardholderName = cardDetails.CardholderName,
                    CardNumber = cardDetails.CardNumber,
                    Csc = cardDetails.CSC,
                    Expires = cardDetails.Expires,
                    ValidFrom = cardDetails.ValidFrom
                });
                return response;
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error in bank Validating Card Details");
                throw;
            }
        }
    }
}
