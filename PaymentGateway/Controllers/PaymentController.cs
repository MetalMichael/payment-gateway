using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models;
using PaymentGateway.Services;

namespace PaymentGateway.Controllers
{
    [Route("api")]
    [ApiController]
    public class PaymentController : ApiControllerBase
    {
        private ILogger<PaymentController> _logger;
        private IBankProvider _bank;
        private IPaymentRequestStore _store;

        public PaymentController(ILogger<PaymentController> logger, IBankProvider bank, IPaymentRequestStore paymentRequestStore)
        {
            _logger = logger;
            _bank = bank;
            _store = paymentRequestStore;
        }

        /// <summary>
        /// Check if a Credit Card's details are valid
        /// </summary>
        /// <param name="cardDetails">Credit Card details to validate</param>
        /// <returns>An object containing the result of the validation</returns>
        [HttpPost("valid")]
        [ProducesResponseType(typeof(CheckCardResult), 200)]
        public async Task<IActionResult> CheckCard([FromBody]CardDetails cardDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                bool valid = await _bank.ValidateCardDetails(cardDetails);
                return Ok(new CheckCardResult(valid));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred Checking Card");
                return InternalServerError();
            }
        }

        /// <summary>
        /// Attempt to charge funds to a Credit Card, with a given Currency and Amount
        /// </summary>
        /// <param name="paymentDetails"></param>
        /// <returns></returns>
        [HttpPost("process")]
        [ProducesResponseType(typeof(ProcessPaymentResult), 200)]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDetails paymentDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Create our own ID to track the request
                var paymentId = Guid.NewGuid();

                PaymentResponse response = await _bank.ProcessPayment(paymentDetails.CardDetails, paymentDetails.TransactionDetails);

                // Store history of both failed and succeeded payments
                var request = new PaymentRequestLog
                {
                    Id = paymentId,
                    MaskedCardDetails = new MaskedCardDetails(paymentDetails.CardDetails),
                    TransactionDetails = paymentDetails.TransactionDetails,
                    PaymentResponse = response
                };
                await _store.LogPaymentRequest(request);

                return Ok(new ProcessPaymentResult(paymentId, response));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred Processing Payment");
                return InternalServerError();
            }
        }

        [HttpGet("find/{id:Guid}")]
        [ProducesResponseType(typeof(PaymentRequestLog), 200)]
        public async Task<IActionResult> FindPayment(Guid id)
        {
            try
            {
                PaymentRequestLog paymentInfo = await _store.FindPaymentRequest(id);

                return Ok(paymentInfo);
            }
            catch(RecordNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred Finding Payment");
                return InternalServerError();
            }
        }
    }
}
