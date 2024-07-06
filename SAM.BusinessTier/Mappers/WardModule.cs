using AutoMapper;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Payload.Wards;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class WardModule : Profile
    {
        public WardModule() {
            CreateMap<Ward, GetWardResponse>();
            CreateMap<CreateNewWardRequest, Ward>();
        }
    }
}
