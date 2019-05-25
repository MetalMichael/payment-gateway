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
    public class PaymentController : ControllerBase
    {
        private IBankProvider _bank;

        public PaymentController(IBankProvider bank)
        {
            _bank = bank;
        }

        [HttpPost("valid")]
        public async Task<ActionResult<IEnumerable<string>>> CheckCard([FromBody]CardDetails cardDetails)
        {
            // TODO
            return Ok();
        }


    }
}
