using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Inventory
{
    public class UpdateInventoryRequest
    {

        public string? SerialNumber { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

    }
}
