using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class AreaController : BaseController<AreaController>
    {
        readonly IAreaService _areaService;

        public AreaController(ILogger<AreaController> logger, IAreaService areaService) : base(logger)
        {
            _areaService = areaService;
        }
    }
    
}
