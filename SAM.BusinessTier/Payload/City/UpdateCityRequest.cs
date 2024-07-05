using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.City
{
    public class UpdateCityRequest
    {

        public int? UnitId { get; set; }

        public string? Name { get; set; }

        public CityType? Type { get; set; }

        public string? Slug { get; set; }

        public CityStatus? Status { get; set; }

        public string? Latitude { get; set; }

        public string? Longitude { get; set; }

        public string? NameEn { get; set; }
    }
}
