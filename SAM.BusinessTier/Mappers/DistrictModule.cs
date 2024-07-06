using AutoMapper;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Payload.Machinery;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class DistrictModule : Profile
    {
        public DistrictModule() {
            CreateMap<District, GetDistrictResponse>();
            CreateMap<CreateNewDistrictRequest, District>();
        }
    }
}
