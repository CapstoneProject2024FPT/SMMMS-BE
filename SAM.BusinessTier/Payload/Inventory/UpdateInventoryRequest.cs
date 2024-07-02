﻿using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Inventory
{
    public class UpdateInventoryRequest
    {

        public InventoryStatus? Status { get; set; }

        public InventoryType? Type { get; set; }

    }
}
