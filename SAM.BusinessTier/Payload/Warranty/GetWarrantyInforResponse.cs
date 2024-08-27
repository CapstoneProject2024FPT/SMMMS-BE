using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Address;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.News;
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

        public WarrantyType Type { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public WarrantyStatus Status { get; set; }

        public string? Description { get; set; }

        public string? Comments { get; set; }

        public DateTime? NextMaintenanceDate { get; set; }

        public int? Priority { get; set; }
        public Guid OrderId { get; set; }

        public InventoryResponse Inventory { get; set; }
        public Dictionary<WarrantyDetailStatus, int>? WarrantyDetai { get; set; }

        public AccountPhoneNumberResponse Customer { get; set; }
        public GetAddressResponse Address { get; set; }
    }

}
