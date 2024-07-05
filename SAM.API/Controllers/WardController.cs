using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Wards;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class WardController : BaseController<WardController>
    {
        readonly IWardService _wardService;

        public WardController(ILogger<WardController> logger, IWardService wardsService) : base(logger)
        {
            _wardService = wardsService;
        }
        [HttpPost(ApiEndPointConstant.Ward.WardsEndPoint)]
        public async Task<IActionResult> CreateNewWard(CreateNewWardRequest createNewWardRequest)
        {
            var response = await _wardService.CreateNewWard(createNewWardRequest);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Ward.WardsEndPoint)]
        public async Task<IActionResult> GetWardList([FromQuery] WardFilter filter)
        {
            var response = await _wardService.GetWardList(filter);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Ward.WardEndPoint)]
        public async Task<IActionResult> GetWardById(Guid id)
        {
            var response = await _wardService.GetWardById(id);
            return Ok(response);
        }

        [HttpPut(ApiEndPointConstant.Ward.WardEndPoint)]
        public async Task<IActionResult> UpdateWard(Guid id, UpdateWardRequest updateWardRequest)
        {
            var isSuccessful = await _wardService.UpdateWard(id, updateWardRequest);
            if (!isSuccessful) return Ok(MessageConstant.Ward.UpdateWardFailedMessage);
            return Ok(MessageConstant.Ward.UpdateWardSuccessMessage);
        }

        [HttpDelete(ApiEndPointConstant.Ward.WardEndPoint)]
        public async Task<IActionResult> RemoveWardStatus(Guid id)
        {
            var isSuccessful = await _wardService.RemoveWardStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Ward.UpdateWardFailedMessage);
            return Ok(MessageConstant.Ward.UpdateWardSuccessMessage);
        }
    }
}
