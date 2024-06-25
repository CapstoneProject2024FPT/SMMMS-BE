using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class BrandController : BaseController<BrandController>  
    {
        readonly IBrandService _brandService;
        public BrandController(ILogger<BrandController> logger, IBrandService brandService) : base(logger)
        {
            _brandService = brandService;
        }
    }
}
