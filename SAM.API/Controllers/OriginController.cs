using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class OriginController : BaseController<OriginController>
    {
        readonly IOriginService _originService;

        public OriginController(ILogger<OriginController> logger, IOriginService originService) : base(logger)
        {
            _originService = originService;
        }
    }
}
