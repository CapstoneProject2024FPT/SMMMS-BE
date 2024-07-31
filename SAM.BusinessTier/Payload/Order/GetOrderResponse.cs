using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Order
{
    public class GetOrderResponse
    {
        public GetOrderResponse() { }
        public Guid? Id { get; set; }
        public string? InvoiceCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public double? TotalAmount { get; set; }
        //public double? Discount { get; set; }

        public Dictionary<NoteStatus, int>? Note { get; set; }
        public double? FinalAmount { get; set; }
        //public string? Note { get; set; }
        public string? Description { get; set; }
        public OrderStatus? Status { get; set; }

        public GetOrderResponse(Guid? id, string? invoiceCode, DateTime? createdDate, DateTime? completedDate, double? totalAmount, double? finalAmount, OrderStatus? status)
        {
            Id = id;
            InvoiceCode = invoiceCode;
            CreatedDate = createdDate;
            CompletedDate = completedDate;
            TotalAmount = totalAmount;
            FinalAmount = finalAmount;
            Status = status;
        }
    }
}
