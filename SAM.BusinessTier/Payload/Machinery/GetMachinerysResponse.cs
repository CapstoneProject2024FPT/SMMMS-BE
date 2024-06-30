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

        public int? Quantity { get; set; }

        public OriginAllResponse? Origin { get; set; }

        public BrandAllResponse? Brand { get; set; }

        public CategoryAllResponse? Category { get; set; }
        
        public List<MachineryImagesAllResponse>? Image { get; set; } = new List<MachineryImagesAllResponse>();

        public List<SpecificationsAllResponse>? Specifications { get; set; } = new List<SpecificationsAllResponse>();
    }
    public class SpecificationsAllResponse
    {
        public Guid? SpecificationId { get; set; }
        public Guid? MachineryId { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }


    }
    public class CategoryAllResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public CategoryType? Type { get; set; }
    }
    public class BrandAllResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateDate { get; set; }
    }
    public class OriginAllResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateDate { get; set; }
    }
    public class MachineryImagesAllResponse
    {
        public string? ImageURL { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
