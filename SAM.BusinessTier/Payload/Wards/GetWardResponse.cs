using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Wards
{
    public class GetWardResponse
    {
        public Guid Id { get; set; }
        public int? UnitId { get; set; }
        public string? Name { get; set; }
        public WardStatus? Status { get; set; }

    }
    public class DistrictResponse
    {
        public Guid Id { get; set; }
        public int? UnitId { get; set; }
        public string? Name { get; set; }

    }
}
