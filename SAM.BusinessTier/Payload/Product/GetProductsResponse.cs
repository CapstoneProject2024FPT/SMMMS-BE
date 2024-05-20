using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Product
{
    public class GetProductsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public double? UnitPrice { get; set; }
        public string Description { get; set; }
        public ProductStatus Status { get; set; }
        public Guid CategoryId { get; set; }
        public int Priority { get; set; }
    }
}
