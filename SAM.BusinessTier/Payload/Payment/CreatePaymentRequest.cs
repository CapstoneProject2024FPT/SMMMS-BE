using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.VNPay
{
    public class CreatePaymentRequest
    {
        public Guid OrderId { get; set; }
        public double Amount { get; set; }
        public PaymentType PaymentType { get; set; }

        public string CallbackUrl { get; set; }
        //public Guid PaymentId { get; set; }
    }
}
