using SAM.BusinessTier.Enums;
using SAM.BusinessTier.Payload.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Machinery
{
    public class GetMachinerySpecificationsRespone
    {
        public string? Name { get; set; }

        public string? Origin { get; set; }

        public string? Model { get; set; }

        public string? Description { get; set; }

        public MachineryStatus? Status { get; set; }

        public List<MachineryImagesResponse>? Image { get; set; } = new List<MachineryImagesResponse>();

        public List<SpecificationsResponse>? Specifications { get; set; } = new List<SpecificationsResponse>();
        public int? Quantity { get; set; }

        public string? SerialNumber { get; set; }

        public double? SellingPrice { get; set; }

        public int? Priority { get; set; }
        public string? Brand { get; set; }

        public string? ControlSystem { get; set; }

        public int? TimeWarranty { get; set; }

        public CategoryResponse? Category { get; set; }
    }
    public class SpecificationsResponse
    {
        public Guid? SpecificationId { get; set; }
        public Guid? MachineryId { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        public string Unit { get; set; }


    }
    public class CategoryResponse
    {
        public string? Name { get; set; }
        public CategoryType? Type { get; set; }
    }
    public class MachineryImagesResponse
    {
        public string? ImageURL { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
