using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Payload.Wards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.City
{
    public class GetCityResponse
    {
        public Guid Id { get; set; }

        public int? UnitId { get; set; }

        public string? Name { get; set; }


        public CityStatus? Status { get; set; }

        public List<DistrictResponse>? District { get; set; } = new List<DistrictResponse>();
    }
}
