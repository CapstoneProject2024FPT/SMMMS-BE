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


        public string? SerialNumber { get; set; }

        public double? SellingPrice { get; set; }

        public int? Priority { get; set; }

        public int? TimeWarranty { get; set; }

        public DateTime? CreateDate { get; set; }

        public GetOriginResponse? Origin { get; set; }

        public GetBrandResponse? Brand { get; set; }

        public GetCategoryResponse? Category { get; set; }

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
    public class GetCategoryResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public CategoryType? Type { get; set; }
    }
    public class GetBrandResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }
    public class GetOriginResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }
    public class MachineryImagesResponse
    {
        public string? ImageURL { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
