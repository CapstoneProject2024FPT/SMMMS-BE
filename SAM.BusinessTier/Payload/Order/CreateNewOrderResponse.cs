﻿using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Order
{
    public class CreateNewOrderResponse
    {
        public List<OrderMachinery> MachineryList { get; set; } = new List<OrderMachinery>();

        public string? Note { get; set; }
        public double? FinalAmount { get; set; }
        public double? TotalAmount { get; set; }
        public string? Description { get; set; }
        public Guid AddressId { get; set; }

    }
    public class OrderMachinery
    {
        public Guid? MachineryId { get; set; }
        public int? Quantity { get; set; }
        public float? StockPrice { get; set; }
        public float? SellingPrice { get; set; }
    }

}
