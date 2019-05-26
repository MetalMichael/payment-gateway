using System;

namespace PaymentGateway.Models
{
    /// <summary>
    /// An Exception that occurs when a record is not found.
    /// Custom exception to avoid pollution using storage specific concepts
    /// </summary>
    public class RecordNotFoundException : Exception
    {
    }
}
