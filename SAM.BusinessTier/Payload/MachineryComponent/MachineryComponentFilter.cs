using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.MachineryComponent
{
    public class MachineryComponentFilter
    {

        public string? Name { get; set; }

        public string? Description { get; set; }
        public ComponentStatus? Status { get; set; }

        public int? TimeWarranty { get; set; }
        public DateTime? CreateDate { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? BrandId { get; set; }

        public Guid? OriginId { get; set; }
    }
}
