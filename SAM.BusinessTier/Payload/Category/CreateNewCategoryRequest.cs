﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Category
{
    public class CreateNewCategoryRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? Kind { get; set; }
        public Guid? MasterCategoryId { get; set; }

    }
}
