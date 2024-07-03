using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class DistrictsController : BaseController<DistrictsController>
    {
        readonly IDistrictsService _districtsService;

        public DistrictsController(ILogger<DistrictsController> logger, IDistrictsService districtsService) : base(logger)
        {
            _districtsService = districtsService;
        }
    }
}
