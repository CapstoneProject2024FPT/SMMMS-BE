using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.VNPay
{
    public class CreatePaymentRequest
    {
        public string? Message { get; set; }
        public string? Url { get; set; }
    }
}
