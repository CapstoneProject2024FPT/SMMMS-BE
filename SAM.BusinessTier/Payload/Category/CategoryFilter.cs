using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Category
{
    public class CategoryFilter
    {
        public string? name { get; set; }
        public CategoryStatus? status { get; set; }
        public CategoryType? type { get; set; }
        public Guid? parentCategoryId { get; set; }

    }
}
