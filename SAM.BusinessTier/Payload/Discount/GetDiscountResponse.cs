using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Discount
{
    public class GetDiscountResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public string? Type { get; set; }

        public string? Status { get; set; }

        public int? Value { get; set; }

        public DateTime? CreateDate { get; set; }

        public CategoriesResponse? Categories { get; set; }

    }
    public class CategoriesResponse
    {
        public string? Name { get; set; }
    }
}
