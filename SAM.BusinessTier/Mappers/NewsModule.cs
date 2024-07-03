using AutoMapper;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.Inventory;
using SAM.BusinessTier.Payload.News;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class NewsModule : Profile
    {
        public NewsModule() {
            CreateMap<GetNewsResponse, News>();
        }
    }
}
