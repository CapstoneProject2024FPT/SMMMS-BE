﻿using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Address
{
    public class AddressFilter
    {

        public string? Name { get; set; }

        public AddressStatus? Status { get; set; }

        public string? Note { get; set; }

        public string? Description { get; set; }

        public Guid? CityId { get; set; }

        public Guid? DistrictId { get; set; }

        public Guid? WardId { get; set; }

        public Guid? AccountId { get; set; }

        public string? NamePersonal {  get; set; }
        public string? PhoneNumber { get; set; }
    }
}
