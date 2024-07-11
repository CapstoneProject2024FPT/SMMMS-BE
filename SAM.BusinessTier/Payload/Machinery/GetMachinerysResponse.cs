using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Machinery
{
    public class GetMachinerysResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }

        public string? Model { get; set; }

        public string? Description { get; set; }

        public MachineryStatus? Status { get; set; }


        public double? SellingPrice { get; set; }

        public int? Priority { get; set; }

        public int? TimeWarranty { get; set; }

        public DateTime? CreateDate { get; set; }

        public Dictionary<InventoryStatus, int>? Quantity { get; set; }

        public OriginResponse? Origin { get; set; }

        public BrandResponse? Brand { get; set; }

        public CategoryResponse? Category { get; set; }
        
        public List<MachineryImagesResponse>? Image { get; set; } = new List<MachineryImagesResponse>();

        public List<SpecificationsResponse>? Specifications { get; set; } = new List<SpecificationsResponse>();
    }
}
