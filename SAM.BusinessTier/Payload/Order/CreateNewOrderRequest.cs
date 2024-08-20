using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Order
{
    public class CreateNewOrderResquest
    {
        public List<OrderMachinery> MachineryList { get; set; } = new List<OrderMachinery>();

        public string? Note { get; set; }
        public string? Description { get; set; }
        public Guid AddressId { get; set; }
        public double? TotalAmountOrder { get; set; }
        public double? FinalAmountOrder { get; set; }



    }
    public class OrderMachinery
    {
        public Guid? MachineryId { get; set; }
        public double? SellingPrice { get; set; }
        public double? TotalAmount { get; set; }
        public double? FinalAmount { get; set; }
        public int? Quantity { get; set; }
    }

}
