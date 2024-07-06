﻿using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Wards
{
    public class WardFilter
    {
        public int? UnitId { get; set; }
        public string? Name { get; set; }
        public WardStatus? Status { get; set; }
        public Guid? DistrictId { get; set; }
    }
}