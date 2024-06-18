using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    public class NewsController : BaseController<NewsController>
    {
        private readonly INewsService _newsService;

        public NewsController(ILogger<NewsController> logger, INewsService newsService) : base(logger)
        {
            _newsService = newsService;
        }

    }
}
