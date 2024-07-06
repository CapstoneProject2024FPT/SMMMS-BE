using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Address
{
    public class CreateNewAddressRequest
    {

        public string? Name { get; set; }

        public string? Note { get; set; }

        public Guid? CityId { get; set; }

        public Guid? DistrictId { get; set; }

        public Guid? WardId { get; set; }

        //public Guid? AccountId { get; set; }
    }
}
