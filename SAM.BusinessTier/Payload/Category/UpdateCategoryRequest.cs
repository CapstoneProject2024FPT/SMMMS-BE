﻿using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Category
{
    public class UpdateCategoryRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public CategoryStatus? Status { get; set; }
        public CategoryKind? Kind { get; set; }
        public Guid? MasterCategoryId { get; set; }

    }
}
