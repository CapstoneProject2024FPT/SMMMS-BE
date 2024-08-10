using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Machinery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.MachineryComponent
{
    public class CreateNewMachineryComponentRequest
    {

        public string? Name { get; set; }

        public string? Description { get; set; }
        public int? Quantity { get; set; }

        public ComponentStatus? Status { get; set; }

        public double? StockPrice { get; set; }

        public double? SellingPrice { get; set; }

        public int? TimeWarranty { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? BrandId { get; set; }

        public Guid? OriginId { get; set; }
        public List<ImageComponentRequest>? Image { get; set; } = new List<ImageComponentRequest>();

    }
    public class ImageComponentRequest
    {
        public string? ImageUrl { get; set; }
    }
}
