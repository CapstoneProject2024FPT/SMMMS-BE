using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Inventory;
using SAM.BusinessTier.Payload.Order;
using SAM.BusinessTier.Payload.Rank;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;
using SAM.DataTier.Paginate;

namespace SAM.API.Controllers
{
    [ApiController]
    public class RankController : BaseController<RankController>
    {
        private readonly IRankService _rankService;

        public RankController(ILogger<RankController> logger, IRankService rankService) : base(logger)
        {
            _rankService = rankService;
        }
        [HttpPost(ApiEndPointConstant.Rank.RanksEndPoint)]
        public async Task<IActionResult> CreateNewRank(CreateNewRankRequest createNewRankRequest)
        {
            var response = await _rankService.CreateNewRank(createNewRankRequest);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Rank.RanksEndPoint)]
        public async Task<IActionResult> GetRankList([FromQuery] RankFilter filter)
        {
            var response = await _rankService.GetRankList(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Rank.RankEndPoint)]
        public async Task<IActionResult> GetRankById(Guid id)
        {
            var response = await _rankService.GetRankById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Rank.RankEndPoint)]
        public async Task<IActionResult> UpdateRank(Guid id, UpdateRankRequest updateRankRequest)
        {
            var isSuccessful = await _rankService.UpdateRank(id, updateRankRequest);
            if (!isSuccessful) return Ok(MessageConstant.Rank.UpdateRankFailedMessage);
            return Ok(MessageConstant.Rank.UpdateRankSuccessMessage);
        }

        [HttpDelete(ApiEndPointConstant.Rank.RankEndPoint)]
        public async Task<IActionResult> RemoveRankStatus(Guid id)
        {
            var isSuccessful = await _rankService.RemoveRankStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Rank.UpdateRankFailedMessage);
            return Ok(MessageConstant.Rank.UpdateRankSuccessMessage);
        }

        [HttpGet(ApiEndPointConstant.Rank.RanksEndPointRankToAccount)]
        public async Task<IActionResult> GetAccountInforInRank(Guid id, [FromQuery] PagingModel pagingModel)
        {
            var response = await _rankService.GetAccountInforInRank(id, pagingModel);
            return Ok(response);
        }
    }
}
