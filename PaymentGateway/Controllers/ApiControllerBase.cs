using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Controllers
{
    /// <summary>
    /// Base class for API Controllers
    /// </summary>
    public class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// Return an Internal Server Error response
        /// </summary>
        protected IActionResult InternalServerError()
        {
            return new StatusCodeResult(500);
        }
    }
}
