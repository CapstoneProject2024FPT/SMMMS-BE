using AutoMapper;
using SAM.BusinessTier.Enums;
using SAM.BusinessTier.Payload.Order;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class OrderModule : Profile
    {
        public OrderModule()
        {
            CreateMap<Order, GetOrderResponse>();
            CreateMap<OrderHistory, GetOrderHistoriesResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.FullName));


        }
    }
}
