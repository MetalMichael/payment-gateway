using Microsoft.AspNetCore.Mvc;
using PaymentGateway.SharedModels;

namespace BankService.Controllers
{
    // TODO: This would normally use its own models!
    [Route("api/")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost("check")]
        public IActionResult CheckCard([FromBody] CardDetails details)
        {
            return Ok(true);
        }

        [HttpPost("pay")]
        public IActionResult ProcessPayment([FromBody] TransactionDetails transaction)
        {
            return Ok(true);
        }
    }
}
