using SAM.BusinessTier.Enums;
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
        public Guid? ParentCategoryId { get; set; }

    }
}
