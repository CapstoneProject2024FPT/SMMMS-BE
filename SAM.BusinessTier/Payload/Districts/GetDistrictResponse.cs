using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Machinery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Districts
{
    public class GetDistrictResponse
    {
        public Guid Id { get; set; }
        public int? UnitId { get; set; }

        public string? Name { get; set; }

        public DistrictStatus? Status { get; set; }

        public CityResponse? City { get; set; }


    }
    public class CityResponse
    {
        public Guid Id { get; set; }
        public int? UnitId { get; set;}
        public string? Name { get; set; }

    }
    public class WardResponse
    {
        public Guid Id { get; set; }
        public int? UnitId { get; set; }
        public string? Name { get; set; }
    }
}
