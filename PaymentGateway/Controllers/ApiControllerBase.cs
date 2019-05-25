using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult InternalServerError()
        {
            return new StatusCodeResult(500);
        }
    }
}
