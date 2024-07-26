using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.MachineryComponent;
using SAM.BusinessTier.Payload.Warranty;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class WarrantyController : BaseController<WarrantyController>
    {
        private readonly IWarrantyService _warrantyService;

        public WarrantyController(ILogger<WarrantyController> logger, IWarrantyService warrantyService) : base(logger)
        {
            _warrantyService = warrantyService;
        }
        [HttpPost(ApiEndPointConstant.Warranty.WarrantiesEndPoint)]
        public async Task<IActionResult> CreateNewWarranty(CreateNewWarrantyRequest createNewWarrantyRequest)
        {
            var response = await _warrantyService.CreateNewWarranty(createNewWarrantyRequest);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Warranty.WarrantiesEndPoint)]
        public async Task<IActionResult> GetWarrantyList([FromQuery] WarrantyFilter filter)
        {
            var response = await _warrantyService.GetWarrantyList(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Warranty.WarrantyEndPoint)]
        public async Task<IActionResult> GetWarrantyById(Guid id)
        {
            var response = await _warrantyService.GetWarrantyById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Warranty.WarrantyEndPoint)]
        public async Task<IActionResult> UpdateWarranty(Guid id, UpdateWarrantyRequest updateWarrantyRequest)
        {
            var isSuccessful = await _warrantyService.UpdateWarranty(id, updateWarrantyRequest);
            if (!isSuccessful) return Ok(MessageConstant.Warranty.UpdateWarrantyFailedMessage);
            return Ok(MessageConstant.Warranty.UpdateWarrantySuccessMessage);
        }
        //    [HttpDelete(ApiEndPointConstant.Warranty.WarrantyEndPoint)]
        //    public async Task<IActionResult> RemoveWarrantyStatus(Guid id)
        //    {
        //        var isSuccessful = await _warrantyService.RemoveWarrantyStatus(id);
        //        if (!isSuccessful) return Ok(MessageConstant.Warranty.UpdateWarrantyFailedMessage);
        //        return Ok(MessageConstant.Warranty.UpdateWarrantySuccessMessage);
        //    }
    }
}
