using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.City
{
    public class CreateNewCityRequest
    {


        public int? UnitId { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public string? Slug { get; set; }

        public string? Latitude { get; set; }

        public string? Longitude { get; set; }

        public string? NameEn { get; set; }
    }
}
