using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Models;
using PaymentGateway.Services;

namespace PaymentGateway.Controllers
{
    [Route("api")]
    [ApiController]
    public class PaymentController : ApiControllerBase
    {
        private IBankProvider _bank;

        public PaymentController(IBankProvider bank)
        {
            _bank = bank;
        }

        [HttpPost("valid")]
        public async Task<IActionResult> CheckCard([FromBody]CardDetails cardDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var valid = await _bank.ValidateCardDetails(cardDetails);
                return Ok(new CheckCardResult(valid));
            }
            catch (Exception e)
            {
                // TODO: Log
                return InternalServerError();
            }
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDetails paymentDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _bank.ProcessPayment(paymentDetails.CardDetails, paymentDetails.TransactionDetails);

                // Store history of both failed and succeeded payments
                var maskedDetails = new MaskedCardDetails(paymentDetails.CardDetails);
                await _store.LogPaymentRequest();
            }
            catch (Exception e)
            {
                // TODO: Log
                return InternalServerError();
            }
        }
    }
}
