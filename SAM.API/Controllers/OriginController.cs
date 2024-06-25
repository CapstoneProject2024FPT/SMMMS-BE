using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Origin;
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
        [HttpPost(ApiEndPointConstant.Origin.OriginsEndPoint)]
        public async Task<IActionResult> CreateNewOrigin(CreateNewOriginRequest createNewOriginRequest)
        {
            var response = await _originService.CreateNewOrigin(createNewOriginRequest);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Origin.OriginsEndPoint)]
        public async Task<IActionResult> GetOriginList([FromQuery] OriginFilter filter)
        {
            var response = await _originService.GetOriginList(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Origin.OriginEndPoint)]
        public async Task<IActionResult> GetOriginById(Guid id)
        {
            var response = await _originService.GetOriginById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Origin.OriginEndPoint)]
        public async Task<IActionResult> UpdateOrigin(Guid id, UpdateOriginRequest updateOriginRequest)
        {
            var isSuccessful = await _originService.UpdateOrigin(id, updateOriginRequest);
            if (!isSuccessful) return Ok(MessageConstant.Origin.UpdateOriginFailedMessage);
            return Ok(MessageConstant.Origin.UpdateOriginSuccessMessage);
        }
        [HttpDelete(ApiEndPointConstant.Origin.OriginEndPoint)]
        public async Task<IActionResult> RemoveBrandStatus(Guid id)
        {
            var isSuccessful = await _originService.RemoveOriginStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Origin.UpdateOriginFailedMessage);
            return Ok(MessageConstant.Origin.UpdateOriginSuccessMessage);
        }
    }
}
