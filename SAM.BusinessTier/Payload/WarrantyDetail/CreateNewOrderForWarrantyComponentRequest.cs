using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.WarrantyDetail
{
    public class CreateNewOrderForWarrantyComponentRequest
    {
        public Guid WarrantyId { get; set; }
        public Guid AccountId { get; set; }
    }   
}
