﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Favorite
{
    public class FavoriteFilter
    {
        public Guid? AccountId { get; set; }
        public Guid? MachineryId { get; set; }
    }
}
