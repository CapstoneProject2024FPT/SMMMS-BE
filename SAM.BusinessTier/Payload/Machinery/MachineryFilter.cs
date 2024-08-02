using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;

namespace SAM.BusinessTier.Payload.Machinery
{
    public class MachineryFilter
    {
        public string? Name { get; set; }

        public List<string>? Model { get; set; }

        public MachineryStatus? Status { get; set; }

        public double? StockPriceRange { get; set; } 

        public double? SellingPriceRange { get; set; } 

        public int? Priority { get; set; }

        public List<Guid>? OriginId { get; set; }

        public List<Guid>? BrandId { get; set; }

        public int? TimeWarranty { get; set; }
        public int? MonthWarrantyNumber { get; set; }

        public List<Guid>? CategoryId { get; set; }

        public List<Guid>? ComponentId { get; set; }
    }
}
