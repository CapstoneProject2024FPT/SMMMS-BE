using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Warranty
{
    public class CreateNewWarrantyRequest
    {

        public string? Description { get; set; }

        public Guid? InventoryId { get; set; }

        public Guid? AddressId { get; set; }

        public Guid? AccountId { get; set; }
    }
}
