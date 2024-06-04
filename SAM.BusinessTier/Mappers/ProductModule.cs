using AutoMapper;
using SAM.BusinessTier.Enums;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.Product;
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
            CreateMap<Machinery, GetProductsResponse>();
            CreateMap<CreateNewProductRequest, Machinery>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ProductStatus.Active.GetDescriptionFromEnum()));
        }
    }
}
