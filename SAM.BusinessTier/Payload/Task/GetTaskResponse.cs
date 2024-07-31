using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Address;
using SAM.BusinessTier.Payload.News;
using SAM.BusinessTier.Payload.Order;
using SAM.BusinessTier.Payload.Warranty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Task
{
    public class GetTaskResponse
    {
        public Guid Id { get; set; }

        public TaskType? Type { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? ExcutionDate { get; set; }

        public TaskManagerStatus? Status { get; set; }

        public DateTime? CompletedDate { get; set; }

        public WarrantyDetailResponse? WarrantyDetail { get; set; }

        public OrderResponse? Order { get; set; }

        public AccountResponse? Staff { get; set; }

        public GetAddressResponse? Address { get; set; }
    }
    public class OrderResponse
    {
        public Guid? Id { get; set; }
        public string? InvoiceCode { get; set; }
        public double? FinalAmount { get; set; }
    }
}
