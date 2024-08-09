using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.WarrantyDetail
{
    public class InventoryUpdate
    {
        public Guid? NewInventoryId { get; set; }
        public Guid? OldInventoryId { get; set; }

        
    }

    public class UpdateWarrantyDetailRequest
    {
        public WarrantyDetailStatus? Status { get; set; }
        public string? Description { get; set; }
        public string? Comments { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public Guid? AccountId { get; set; }
        public List<InventoryUpdate>? InventoryUpdates { get; set; } = new List<InventoryUpdate>();
        public string? Image { get; set; }
    }

}
