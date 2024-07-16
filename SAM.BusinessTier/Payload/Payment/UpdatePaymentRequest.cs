using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Payment
{
    public class UpdatePaymentRequest
    {
        public PaymentStatus? Status { get; set; }

        public string? Note { get; set; }
    }
}
