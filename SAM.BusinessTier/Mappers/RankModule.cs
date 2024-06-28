using AutoMapper;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.Rank;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class RankModule : Profile
    {
        public RankModule() {
            CreateMap<CreateNewRankRequest, Rank>();
            CreateMap<Rank, GetRankResponse>();
        }
    }
}
