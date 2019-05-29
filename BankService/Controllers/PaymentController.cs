using Microsoft.AspNetCore.Mvc;
using PaymentGateway.SharedModels;
using System;

namespace BankService.Controllers
{
    // TODO: This would normally use its own models!
    [Route("api/")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        /// <summary>
        /// Stub service to simulate checking a card's details
        /// </summary>
        /// <param name="details">Credit Card Details to validate</param>
        /// <returns>Validity of the Credit Card Details</returns>
        [HttpPost("check")]
        [ProducesResponseType(typeof(bool), 200)]
        public IActionResult CheckCard([FromBody] CardDetails details)
        {
            return Ok(true);
        }

        /// <summary>
        /// Stub service to simulate processing a payment
        /// </summary>
        /// <param name="paymentDetails">Details required to make the payment</param>
        /// <returns>Details of the completed transaction</returns>
        [HttpPost("pay")]
        [ProducesResponseType(typeof(PaymentResponse), 200)]
        public IActionResult ProcessPayment([FromBody] PaymentDetails paymentDetails)
        {
            return Ok(new PaymentResponse
            {
                Successful = true,
                TransactionId = Guid.NewGuid()
            });
        }
    }
}
