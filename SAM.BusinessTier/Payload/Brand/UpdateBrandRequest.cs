using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Brand
{
    public class UpdateBrandRequest
    {
        public string? Name {  get; set; }

        public string? Description { get; set; }

        public BrandStatus? Status { get; set; }

        public string? URLImage { get; set; }
    }
}
