using AutoMapper;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class BrandModule : Profile
    {
        public BrandModule() {
            CreateMap<Brand, GetBrandResponse>();
            CreateMap<CreateNewBrandRequest, Brand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BrandStatus.Active.GetDescriptionFromEnum()))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now));

        }
    }
}
