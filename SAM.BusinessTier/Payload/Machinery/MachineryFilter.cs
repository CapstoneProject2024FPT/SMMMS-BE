using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;

namespace SAM.BusinessTier.Payload.Machinery
{
    public class MachineryFilter
    {
        public List<string>? Name { get; set; }

        public List<string>? Origin { get; set; }

        public List<string>? Model { get; set; }

        public MachineryStatus? Status { get; set; }

        public string? SerialNumber { get; set; }

        public double? StockPriceRange { get; set; } 

        public double? SellingPriceRange { get; set; } 

        public int? Priority { get; set; }

        public List<string>? Brand { get; set; }

        public int? TimeWarranty { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
