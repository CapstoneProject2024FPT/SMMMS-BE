using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class WarrantyController : BaseController<WarrantyController>
    {
        readonly IWarrantyService _warrantyService;

        public WarrantyController(ILogger<WarrantyController> logger, IWarrantyService warrantyService) : base(logger)
        {
            _warrantyService = warrantyService;
        }
    }
}
