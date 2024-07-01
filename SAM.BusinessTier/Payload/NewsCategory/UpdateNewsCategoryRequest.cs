using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.NewsCategory
{
    public class UpdateNewsCategoryRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public NewsCategoryStatus? Status { get; set; }

    }
}
