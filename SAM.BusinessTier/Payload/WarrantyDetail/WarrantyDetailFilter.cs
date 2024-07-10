using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.WarrantyDetail
{
    public class WarrantyDetailFilter
    {
        public WarrantyDetailType? Type { get; set; }

        public WarrantyDetailStatus? Status { get; set; }

        public string? Description { get; set; }

        public string? Comments { get; set; }

        public Guid? AccountId { get; set; }

        public Guid? WarrantyId { get; set; }

    }
}
