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
    public class GetDetailWarrantyInfor
    {
        public Guid? Id { get; set; }

        public WarrantyType? Type { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public WarrantyStatus Status { get; set; }

        public string? Description { get; set; }

        public string? Comments { get; set; }

        public DateTime? NextMaintenanceDate { get; set; }

        public int? Priority { get; set; }
        public Guid OrderId { get; set; }
        public List<WarrantyDetailResponse>? WarrantyDetail { get; set; } = new List<WarrantyDetailResponse>();

        public InventoryResponse? Inventory { get; set; }

        public AccountResponse? Customer { get; set; }
        public GetAddressResponse? Address { get; set; }


    }
    public class WarrantyDetailResponse
    {
        public Guid? Id { get; set; }
        public WarrantyDetailStatus? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string? Description { get; set; }
        public string? Comments { get; set; }
        public Guid? WarrantyId { get; set; }

        public Guid? AccountId { get; set; }
    }
    public class InventoryResponse
    {
        public Guid? Id { get; set; }
        public string? SerialNumber { get; set; }

        public InventoryType? Type { get; set; }

        public GetMachinerySpecificationsRespone Machinery { get; set; }

    }
}
