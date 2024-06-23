using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Services.Interfaces;

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
    }
}
