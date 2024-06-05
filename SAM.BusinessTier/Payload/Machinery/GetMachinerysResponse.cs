using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Machinery
{
    public class GetMachinerysResponse
    {
        public string? Name { get; set; }

        public string? Origin { get; set; }

        public string? Model { get; set; }

        public string? Description { get; set; }

        public MachineryStatus? Status { get; set; }

        public string? SerialNumber { get; set; }

        public double? SellingPrice { get; set; }

        public int? Priority { get; set; }

        public Guid? CategoryId { get; set; }

    }
}
