using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Favorite;
using SAM.BusinessTier.Payload.Inventory;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class FavoriteController : BaseController<FavoriteController>
    {
        readonly IFavoriteService _favoriteService;

        public FavoriteController(ILogger<FavoriteController> logger, IFavoriteService favoriteService) : base(logger)
        {
            _favoriteService = favoriteService;
        }
        [HttpPost(ApiEndPointConstant.Favorite.FavoritesEndPoint)]
        public async Task<IActionResult> CreateNewFavorite(CreateNewFavoriteRequest createNewFavoriteRequest)
        {
            var response = await _favoriteService.CreateNewFavorite(createNewFavoriteRequest);
            return Ok(response);
        }
        [HttpDelete(ApiEndPointConstant.Favorite.FavoriteEndPoint)]
        public async Task<IActionResult> DeleteFavorite(Guid id)
        {
            var isSuccessful = await _favoriteService.DeleteFavorite(id);
            if (!isSuccessful) return Ok(MessageConstant.Favorite.DeleteFailedMessage);
            return Ok(MessageConstant.Favorite.DeleteSuccessMessage);
        }

        [HttpGet(ApiEndPointConstant.Favorite.FavoritesEndPoint)]
        public async Task<IActionResult> GetFavoriteById()
        {
            var response = await _favoriteService.GetFavoriteById();
            return Ok(response);
        }

    }
}
