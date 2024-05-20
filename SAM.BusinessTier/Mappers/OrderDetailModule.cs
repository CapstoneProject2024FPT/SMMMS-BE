using AutoMapper;
using SAM.BusinessTier.Payload.Order;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class OrderDetailModule : Profile
    {
        public OrderDetailModule()
        {
            CreateMap<Order, GetOrderDetailResponse>();
        }
    }
}
