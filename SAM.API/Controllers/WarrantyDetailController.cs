using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Warranty;
using SAM.BusinessTier.Payload.WarrantyDetail;
using SAM.BusinessTier.Services.Implements;
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
        //[HttpPost(ApiEndPointConstant.WarrantyDetail.WarrantyDetailsEndPoint)]
        //public async Task<IActionResult> CreateNewWarrantyDetail(CreateNewWarrantyDetailRequest createNewWarrantyDetailRequest)
        //{
        //    var response = await _warrantyDetailService.CreateNewWarrantyDetail(createNewWarrantyDetailRequest);
        //    return Ok(response);
        //}
        [HttpGet(ApiEndPointConstant.WarrantyDetail.WarrantyDetailsEndPoint)]
        public async Task<IActionResult> GetWarrantyDetailList([FromQuery] WarrantyDetailFilter filter)
        {
            var response = await _warrantyDetailService.GetWarrantyDetailList(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.WarrantyDetail.WarrantydetailEndPoint)]
        public async Task<IActionResult> GetWarrantyDetailById(Guid id)
        {
            var response = await _warrantyDetailService.GetWarrantyDetailById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.WarrantyDetail.WarrantydetailEndPoint)]
        public async Task<IActionResult> UpdateWarrantyDetail(Guid id, UpdateWarrantyDetailRequest updateWarrantyDetailRequest)
        {
            var isSuccessful = await _warrantyDetailService.UpdateWarrantyDetail(id, updateWarrantyDetailRequest);
            if (!isSuccessful) return Ok(MessageConstant.WarrantyDetail.UpdateWarrantyDetailFailedMessage);
            return Ok(MessageConstant.WarrantyDetail.UpdateWarrantyDetailSuccessMessage);
        }
        [HttpPost(ApiEndPointConstant.WarrantyDetail.WarrantydetailCreateOrderWarrantyChangeEndPoint)]
        public async Task<IActionResult> CreateOrderForReplacedComponents(CreateNewOrderForWarrantyComponentRequest createNewOrderForWarrantyComponent)
        {
            var response = await _warrantyDetailService.CreateOrderForReplacedComponents(createNewOrderForWarrantyComponent);
            return Ok(response);
        }

        //[HttpDelete(ApiEndPointConstant.WarrantyDetail.WarrantydetailEndPoint)]
        //public async Task<IActionResult> RemoveWarrantyDetailStatus(Guid id)
        //{
        //    var isSuccessful = await _warrantyDetailService.RemoveWarrantyDetailStatus(id);
        //    if (!isSuccessful) return Ok(MessageConstant.WarrantyDetail.UpdateWarrantyDetailFailedMessage);
        //    return Ok(MessageConstant.WarrantyDetail.UpdateWarrantyDetailSuccessMessage);
        //}
    }
}
