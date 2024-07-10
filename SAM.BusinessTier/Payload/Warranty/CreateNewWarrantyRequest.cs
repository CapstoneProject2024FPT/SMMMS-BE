using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Warranty
{
    public class CreateNewWarrantyRequest
    {
        public string? Type { get; set; }

        public DateTime? StartDate { get; set; }

        public string? Description { get; set; }

        public int? Priority { get; set; }

        public Guid? InventoryId { get; set; }

        public int? ExecutionTime { get; set; }
    }
}
