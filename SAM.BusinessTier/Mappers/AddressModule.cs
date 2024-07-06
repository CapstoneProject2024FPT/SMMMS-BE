using AutoMapper;
using SAM.BusinessTier.Payload.Address;
using SAM.BusinessTier.Payload.Brand;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class AddressModule : Profile
    {
        public AddressModule() {
            CreateMap<Address, GetAddressResponse>();
            CreateMap<CreateNewAddressRequest, Address>();
        }
    }
}
