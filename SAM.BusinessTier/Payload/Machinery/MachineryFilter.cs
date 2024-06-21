using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Machinery
{
    public class MachineryFilter
    {
        public List<string>? Name { get; set; }

        public List<string>? Origin { get; set; }

        public List<string>? Model { get; set; }

        public MachineryStatus? Status { get; set; }

        public string? SerialNumber { get; set; }

        public double? StockPrice { get; set; }

        public double? SellingPrice { get; set; }

        public int? Priority { get; set; }

        public List<string>? Brand { get; set; }

        public int? TimeWarranty { get; set; }

        public Guid? CategoryId { get; set; }

    }
}
