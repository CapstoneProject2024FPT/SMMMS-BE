
using Microsoft.AspNetCore.Http.Features;
using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Order
{
    public class GetOrderDetailResponse
    {
        public Guid? OrderId { get; set; }
        public string? InvoiceCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public List<OrderDetailResponse>? ProductList { get; set; } = new List<OrderDetailResponse>();
        
        public double? TotalAmount { get; set; }
        //public double? Discount { get; set; }
        public double? FinalAmount { get; set; }
        public string? Note { get; set; }
        public OrderStatus? Status { get; set; }
        public OrderUserResponse? UserInfo { get; set; }
    }

    public class OrderDetailResponse
    {
        public Guid? OrderDetailId { get; set; }
        public Guid? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? Quantity { get; set; }
        public double? SellingPrice { get; set; }
        public double? TotalAmount { get; set; }

    }

    public class OrderUserResponse
    {
        public Guid? Id { get; set; }
        public string? FullName { get; set; }
        public RoleEnum? Role { get; set; }
    }
}
