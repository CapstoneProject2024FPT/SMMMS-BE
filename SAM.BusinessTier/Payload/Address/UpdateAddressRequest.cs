using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Address
{
    public class UpdateAddressRequest
    {
        public string? Name { get; set; }

        public AddressStatus? Status { get; set; }

        public string? Note { get; set; }


        public Guid? CityId { get; set; }

        public Guid? DistrictId { get; set; }

        public Guid? WardId { get; set; }
    }
}
