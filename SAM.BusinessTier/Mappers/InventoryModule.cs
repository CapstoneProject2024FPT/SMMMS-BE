using AutoMapper;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.Inventory;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class InventoryModule : Profile
    {
        public InventoryModule() {
            DateTime currentTime = TimeUtils.GetCurrentSEATime();
            CreateMap<Inventory, GetInventoryResponse>();
            CreateMap<CreateNewInventoryRequest, Inventory>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => TimeUtils.GetTimestamp(currentTime)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => InventoryStautus.Available.GetDescriptionFromEnum()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => InventoryType.Machinery.GetDescriptionFromEnum()))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now));

        }
    }
}
