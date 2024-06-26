﻿using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Inventory
{
    public class GetInventoryResponse
    {
        public Guid Id { get; set; }

        public string? SerialNumber { get; set; }

        public InventoryStautus? Status { get; set; }

        public InventoryType? Type { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? SoldDate { get; set; }


        public Guid? MachineryId { get; set; }
    }
}
