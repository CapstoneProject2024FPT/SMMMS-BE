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

        public double? StockPrice { get; set; }
        public double? SellingPrice { get; set; }
        public double? FinalAmount {  get; set; }
        public int? Discount { get; set; }
        public int? Priority { get; set; }

        public int? TimeWarranty { get; set; }
        public int? MonthWarrantyNumber { get; set; }

        public DateTime? CreateDate { get; set; }

        //public int? Quantity { get; set; }
        public Dictionary<InventoryStatus, int>? Quantity { get; set; }

        //public Dictionary<InventoryStatus, int>? ComponentQuantity { get; set; }

        public OriginResponse? Origin { get; set; }

        public BrandResponse? Brand { get; set; }

        public CategoryResponse? Category { get; set; }

        public List<ComponentResponse>? Component { get; set; } = new List<ComponentResponse>();

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

    }
    public class BrandResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class OriginResponse
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class MachineryImagesResponse
    {
        public Guid? Id { get; set; }
        public string? ImageURL { get; set; }
        public DateTime? CreateDate { get; set; }

    }
    public class ComponentResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public ComponentStatus? Status { get; set; }

        public double? StockPrice { get; set; }

        public double? SellingPrice { get; set; }

    }
}
