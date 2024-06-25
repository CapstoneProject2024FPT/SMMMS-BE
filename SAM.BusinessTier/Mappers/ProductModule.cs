using AutoMapper;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class ProductModule : Profile
    {
        public ProductModule() {
            CreateMap<Machinery, GetMachinerysResponse>();
            CreateMap<CreateNewMachineryRequest, Machinery>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MachineryStatus.Available.GetDescriptionFromEnum()));
            CreateMap<Machinery, GetMachinerySpecificationsRespone>();
        }
    }
}
