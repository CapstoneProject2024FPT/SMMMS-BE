using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Address;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class AdminController : BaseController<AdminController>
    {
        readonly IAdminDashboardService _adminDashboardService;

        public AdminController(ILogger<AdminController> logger, IAdminDashboardService adminDashboardService) : base(logger)
        {
            _adminDashboardService = adminDashboardService;
        }
        [HttpGet(ApiEndPointConstant.Admin.AdminsEndPoint)]
        public async Task<IActionResult> GetYearlyStatistics(int year)
        {
            var response = await _adminDashboardService.GetYearlyStatistics(year);
            return Ok(response);
        }
    }
}
