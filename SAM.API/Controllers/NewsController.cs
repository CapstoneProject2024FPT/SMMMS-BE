using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.News;
using SAM.BusinessTier.Payload.Rank;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class NewsController : BaseController<NewsController>
    {
        private readonly INewsService _newsService;

        public NewsController(ILogger<NewsController> logger, INewsService newsService) : base(logger)
        {
            _newsService = newsService;
        }
        [HttpPost(ApiEndPointConstant.News.NewsSEndPoint)]
        public async Task<IActionResult> CreateNewNews(CreateNewsRequest createNewsRequest)
        {
            var response = await _newsService.CreateNewNews(createNewsRequest);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.News.NewsSEndPoint)]
        public async Task<IActionResult> GetNewsList([FromQuery] NewsFilter filter, [FromQuery] PagingModel pagingModel)
        {
            var response = await _newsService.GetNewsList(filter, pagingModel);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.News.NewsSEndPointNoPaginate)]
        public async Task<IActionResult> GetNewsListNoPagingNate([FromQuery] NewsFilter filter)
        {
            var response = await _newsService.GetNewsListNoPagingNate(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.News.NewsEndPoint)]
        public async Task<IActionResult> GetNewsById(Guid id)
        {
            var response = await _newsService.GetNewsById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.News.NewsEndPoint)]
        public async Task<IActionResult> UpdateNews(Guid id, UpdateNewsRequest updateNewsRequest)
        {
            var isSuccessful = await _newsService.UpdateNews(id, updateNewsRequest);
            if (!isSuccessful) return Ok(MessageConstant.Rank.UpdateRankFailedMessage);
            return Ok(MessageConstant.Rank.UpdateRankSuccessMessage);
        }

        [HttpDelete(ApiEndPointConstant.News.NewsEndPoint)]
        public async Task<IActionResult> RemoveNewsStatus(Guid id)
        {
            var isSuccessful = await _newsService.RemoveNewsStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Rank.UpdateRankFailedMessage);
            return Ok(MessageConstant.Rank.UpdateRankSuccessMessage);
        }

    }
}
