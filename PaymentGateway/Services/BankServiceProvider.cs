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
    /// <summary>
    /// Service to verify and forward payment details with a remote Banking Service
    /// </summary>
    public class BankServiceProvider : IBankProvider
    {
        private ILogger<BankServiceProvider> _log;
        private BankClient _client = new BankClient(Environment.GetEnvironmentVariable("BANK_URL"), new HttpClient());

        /// <summary>
        /// Create a new BankServiceProvider
        /// </summary>
        /// <param name="logger">Logging instance</param>
        public BankServiceProvider(ILogger<BankServiceProvider> logger)
        {
            _log = logger;
        }

        /// <summary>
        /// Verify the validity of a card's details using the Bank Service
        /// </summary>
        /// <param name="cardDetails">Card details to verify</param>
        /// <returns>Validity of the Card Details</returns>
        public async Task<bool> ValidateCardDetailsAsync(CardDetails cardDetails)
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

        /// <summary>
        /// Attempt to process a payment (Card Details and Transaction) using the Bank Service
        /// </summary>
        /// <param name="cardDetails">Card details to use for the payment</param>
        /// <param name="transaction">Currency and amount to process</param>
        /// <returns>Model containing the result of the Payment and, if applicable, Transaction ID</returns>
        public async Task<PaymentResponse> ProcessPaymentAsync(CardDetails cardDetails, TransactionDetails transaction)
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
    }
}
