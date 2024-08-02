using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Discount
{
    public class UpdateDiscountRequest
    {
        public string? Name { get; set; }

        public DiscountType? Type { get; set; }

        public DiscountStatus? Status { get; set; }

        public double? Value { get; set; }

    }
}
