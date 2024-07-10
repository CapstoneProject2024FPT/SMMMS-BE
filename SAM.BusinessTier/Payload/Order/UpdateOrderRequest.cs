using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Order
{
    public class UpdateOrderRequest
    {
        public OrderStatus? Status { get; set; }
        public string? Note { get; set; }

        //public DateTime CompletedDate { get; set; }
    }
}
