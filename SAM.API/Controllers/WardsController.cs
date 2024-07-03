using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class WardsController : BaseController<WardsController>
    {
        readonly IWardsService _wardsService;

        public WardsController(ILogger<WardsController> logger, IWardsService wardsService) : base(logger)
        {
            _wardsService = wardsService;
        }
    }
}
