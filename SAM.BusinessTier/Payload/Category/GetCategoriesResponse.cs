using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Enums.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Category
{
    public class GetCategoriesResponse
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryStatus Status { get; set; }
        public CategoryType Type { get; set; }
        public CategoryKind? Kind { get; set; }
        public Guid? MasterCategoryId { get; set; }
        public DiscountResponse? Discount { get; set; }

    }
    public class DiscountResponse
    {
        public string? Name { get; set; }
        public DiscountType? Type { get; set; }
        public int? Value { get; set; }
    }

}
