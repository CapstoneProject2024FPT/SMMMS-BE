using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Order
{
    public class CreateNewOrderResponse
    {
        public List<OrderProduct> ProductList { get; set; } = new List<OrderProduct>();
        public string? Note { get; set; }

        public double? TotalAmount { get; set; }

        public double? Discount { get; set; }

        public double? FinalAmount { get; set; }

        public Guid? AccountId { get; set; }

    }
    public class OrderProduct
    {
        public Guid MachineryId { get; set; }
        public int Quantity { get; set; }
        public float SellingPrice { get; set; }
        public double TotalAmount { get; set; }
    }
    
}
