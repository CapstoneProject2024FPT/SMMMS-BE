using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class CityController : BaseController<CityController>
    {
        readonly ICityService _cityService;

        public CityController(ILogger<CityController> logger, ICityService cityService) : base(logger)
        {
            _cityService = cityService;
        }
    }
}
