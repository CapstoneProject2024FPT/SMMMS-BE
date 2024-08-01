using AutoMapper;
using SAM.BusinessTier.Payload.Favorite;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class FavoriteModule : Profile
    {
        public FavoriteModule() {
            CreateMap<CreateNewFavoriteRequest, Favorite>();
        }
    }
}
