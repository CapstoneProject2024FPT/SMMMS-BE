using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Warranty
{
    public class GetWarrantyInforResponse
    {
        public Guid? Id { get; set; }

        public string? Type { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public string? Status { get; set; }

        public string? Description { get; set; }

        public string? Comments { get; set; }

        public DateTime? NextMaintenanceDate { get; set; }

        public int? Priority { get; set; }

        public Guid? InventoryId { get; set; }
    }
}
