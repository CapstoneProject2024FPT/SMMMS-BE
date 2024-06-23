using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class MachineryComponentsController : BaseController<MachineryComponentsController>
    {
        private readonly IMachineryComponentService _machineryComponentService;
            public MachineryComponentsController(ILogger<MachineryComponentsController> logger, IMachineryComponentService machineryComponentService) : base(logger)
        {
            _machineryComponentService = machineryComponentService;
        }
    }
}
