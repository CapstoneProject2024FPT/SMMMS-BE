﻿using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Warranty
{
    public class UpdateWarrantyRequest
    {

        public WarrantyStatus? Status { get; set; }

        public string? Description { get; set; }

        public string? Comments { get; set; }

        public int? Priority { get; set; }

    }
}
