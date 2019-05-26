using Microsoft.AspNetCore.Mvc;

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
