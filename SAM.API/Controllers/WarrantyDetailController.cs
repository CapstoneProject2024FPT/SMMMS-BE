using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class WarrantyDetailController : BaseController<WarrantyController>
    {
        readonly IWarrantyDetailService _warrantyDetailService;

        public WarrantyDetailController(ILogger<WarrantyController> logger, IWarrantyDetailService warrantyDetailService) : base(logger)
        {
            _warrantyDetailService = warrantyDetailService;
        }
    }
}
