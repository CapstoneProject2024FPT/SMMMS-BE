using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class DistrictController : BaseController<DistrictController>
    {
        readonly IDistrictService _districtService;

        public DistrictController(ILogger<DistrictController> logger, IDistrictService districtsService) : base(logger)
        {
            _districtService = districtsService;
        }
        [HttpPost(ApiEndPointConstant.District.DistrictsEndPoint)]
        public async Task<IActionResult> CreateNewDistrict(CreateNewDistrictRequest createNewDistrictRequest)
        {
            var response = await _districtService.CreateNewDistrict(createNewDistrictRequest);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.District.DistrictsEndPoint)]
        public async Task<IActionResult> GetDistrictList([FromQuery] DistrictFilter filter)
        {
            var response = await _districtService.GetDistrictList(filter);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.District.DistrictEndPoint)]
        public async Task<IActionResult> GetDistrictById(Guid id)
        {
            var response = await _districtService.GetDistrictById(id);
            return Ok(response);
        }

        [HttpPut(ApiEndPointConstant.District.DistrictEndPoint)]
        public async Task<IActionResult> UpdateDistrict(Guid id, UpdateDistrictRequest updateDistrictRequest)
        {
            var isSuccessful = await _districtService.UpdateDistrict(id, updateDistrictRequest);
            if (!isSuccessful) return Ok(MessageConstant.District.UpdateDistrictFailedMessage);
            return Ok(MessageConstant.District.UpdateDistrictSuccessMessage);
        }

        [HttpDelete(ApiEndPointConstant.District.DistrictEndPoint)]
        public async Task<IActionResult> RemoveDistrictStatus(Guid id)
        {
            var isSuccessful = await _districtService.RemoveDistrictStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.District.UpdateDistrictFailedMessage);
            return Ok(MessageConstant.District.UpdateDistrictSuccessMessage);
        }
    }
}
