﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Discount
{
    public class CreateNewDiscountRequest
    {
        public string? Name { get; set; }
        public string? Type { get; set; }

        public int? Value { get; set; }

    }
}
