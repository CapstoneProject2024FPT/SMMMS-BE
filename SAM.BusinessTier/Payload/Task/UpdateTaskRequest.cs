﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Task
{
    public class UpdateTaskRequest
    {

        public Guid? AccountId { get; set; }

        public Guid? AddressId { get; set; }
        public DateTime? ExcutionDate { get; set; }
    }
}
