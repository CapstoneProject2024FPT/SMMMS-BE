using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Machinery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.MachineryComponent
{
    public class GetMachineryComponentResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
        public int? Quantity { get; set; }

        public DateTime? CreateDate { get; set; }

        public ComponentStatus? Status { get; set; }

        public double? StockPrice { get; set; }

        public double? SellingPrice { get; set; }

        public int? TimeWarranty { get; set; }

        public OriginResponse? Origin { get; set; }

        public BrandResponse? Brand { get; set; }

        public CategoryResponse? Category { get; set; }
        public List<ImageComponentResponse>? Image { get; set; } = new List<ImageComponentResponse>();

    }
    public class ImageComponentResponse
    {
        public Guid? Id { get; set; }
        public string? ImageURL { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
