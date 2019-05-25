using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Models
{
    public class CheckCardResult
    {
        public CheckCardResult(bool valid)
        {
            Valid = valid;
        }

        public bool Valid { get; }
    }
}
