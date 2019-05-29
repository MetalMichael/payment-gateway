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
        [HttpPost("check")]
        [ProducesResponseType(typeof(bool), 200)]
        public IActionResult CheckCard([FromBody] CardDetails details)
        {
            return Ok(true);
        }

        [HttpPost("pay")]
        [ProducesResponseType(typeof(PaymentResponse), 200)]
        public IActionResult ProcessPayment([FromBody] PaymentDetails transaction)
        {
            return Ok(new PaymentResponse
            {
                Successful = true,
                TransactionId = Guid.NewGuid()
            });
        }
    }
}
