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

        public AddressStatus? Status { get; set; }

        public string? Note { get; set; }


        public List<CityResponse>? City { get; set; } = new List<CityResponse>();
        public List<DistrictResponse>? District { get; set; } = new List<DistrictResponse>();
        public List<WardResponse>? Ward { get; set; } = new List<WardResponse>();
        public List<AccountResponse>? Account { get; set; } = new List<AccountResponse>();

    }

}
