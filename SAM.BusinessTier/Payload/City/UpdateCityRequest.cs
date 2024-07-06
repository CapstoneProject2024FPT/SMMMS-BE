﻿using SAM.BusinessTier.Enums.EnumStatus;
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

        public string? Name { get; set; }

        public CityType? Type { get; set; }

        public string? Slug { get; set; }

        public CityStatus? Status { get; set; }

    }
}
