using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Payload.News;
using SAM.BusinessTier.Payload.Wards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Address
{
    public class GetAddressResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }
        public string? NamePersional { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressStatus? Status { get; set; }

        public string? Note { get; set; }
        public CityResponse? City { get; set; }
        public DistrictResponse District { get; set; }
        public WardResponse Ward { get; set; }
        public AccountResponse Account { get; set; }

    }

}
