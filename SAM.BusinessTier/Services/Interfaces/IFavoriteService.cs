using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Favorite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<Guid> CreateNewFavorite(CreateNewFavoriteRequest createNewFavoriteRequest);
        Task<GetFavoriteResponse> GetFavoriteById();
        Task<bool> DeleteFavorite(Guid id);
    }
}
