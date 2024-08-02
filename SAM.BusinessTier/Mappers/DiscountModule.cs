using AutoMapper;
using SAM.BusinessTier.Payload.City;
using SAM.BusinessTier.Payload.Discount;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class DiscountModule : Profile
    {
        public DiscountModule() {
            CreateMap<CreateNewDiscountRequest, Discount>();
        }
    }
}
