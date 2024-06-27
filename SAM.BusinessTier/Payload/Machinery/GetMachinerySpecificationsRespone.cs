using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Category;
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
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Model { get; set; }

        public string? Description { get; set; }

        public MachineryStatus? Status { get; set; }


        public double? SellingPrice { get; set; }

        public int? Priority { get; set; }

        public int? TimeWarranty { get; set; }

        public DateTime? CreateDate { get; set; }

        public OriginResponse? Origin { get; set; }

        public BrandResponse? Brand { get; set; }

        public CategoryResponse? Category { get; set; }

        public List<MachineryImagesResponse>? Image { get; set; } = new List<MachineryImagesResponse>();

        public List<SpecificationsResponse>? Specifications { get; set; } = new List<SpecificationsResponse>();
    }
    public class SpecificationsResponse
    {
        public Guid? SpecificationId { get; set; }
        public Guid? MachineryId { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }


    }
    public class CategoryResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public CategoryType? Type { get; set; }

        public DateTime? CreateDate { get; set; }
    }
    public class BrandResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateDate { get; set; }
    }
    public class OriginResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateDate { get; set; }
    }
    public class MachineryImagesResponse
    {
        public string? ImageURL { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
