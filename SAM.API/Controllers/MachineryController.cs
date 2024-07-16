using DentalLabManagement.API.Controllers;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Category;

using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;
using SAM.DataTier.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.Order;

namespace SAM.API.Controllers
{
    [ApiController]
    public class MachineryController : BaseController<MachineryController>
    {
        private readonly IMachineryService _iMachineryService;

        public MachineryController(ILogger<MachineryController> logger, IMachineryService machineryService) : base(logger)
        {
            _iMachineryService = machineryService;
        }
        [HttpPost(ApiEndPointConstant.Product.MachineriesEndPoint)]
        public async Task<IActionResult> CreateNewMachinerys(CreateNewMachineryRequest mechinery)
        {
            var response = await _iMachineryService.CreateNewMachinerys(mechinery);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Product.MachineryEndPoint)]
        public async Task<IActionResult> GetMachinerySpecificationsDetail(Guid id)
        {
            var response = await _iMachineryService.GetMachinerySpecificationsDetail(id);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Product.MachineriesEndPoint)]
        public async Task<IActionResult> GetMachineryList([FromQuery] MachineryFilter filter, [FromQuery] PagingModel pagingModel)
        {
            var response = await _iMachineryService.GetMachineryList(filter, pagingModel);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Product.MachineriesEndPointNoPaginate)]
        public async Task<IActionResult> GetMachineryListNoPagingNate([FromQuery] MachineryFilter filter)
        {
            var response = await _iMachineryService.GetMachineryListNoPagingNate(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Inventory.MachineriesInventoryDetailEndPoint)]
        public async Task<IActionResult> GetMachineryAndComponents(Guid id)
        {
            var response = await _iMachineryService.GetMachineryAndComponentsByInventoryId(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Product.MachineryEndPoint)]
        public async Task<IActionResult> UpdateMachinery(Guid id, UpdateMachineryRequest updateProductRequest)
        {
            var response = await _iMachineryService.UpdateMachinery(id, updateProductRequest);
            if (!response) return Ok(MessageConstant.Machinery.UpdateMachinerytFailedMessage);
            return Ok(MessageConstant.Machinery.UpdateStatusSuccessMessage);
        }
        [HttpPut(ApiEndPointConstant.Product.MachineriesUpdateStatusEndPoint)]
        public async Task<IActionResult> UpdateStatusMachineryResponse(Guid id, UpdateStatusMachineryResponse updateStatusMachineryResponse)
        {
            var response = await _iMachineryService.UpdateStatusMachineryResponse(id, updateStatusMachineryResponse);
            if (!response) return Ok(MessageConstant.Machinery.UpdateMachinerytFailedMessage);
            return Ok(MessageConstant.Machinery.UpdateStatusSuccessMessage);
        }
        [HttpDelete(ApiEndPointConstant.Product.MachineryEndPoint)]
        public async Task<IActionResult> RemoveMachineryStatus(Guid id)
        {
            var response = await _iMachineryService.RemoveMachineryStatus(id);
            if (!response) return Ok(MessageConstant.Machinery.UpdateMachinerytFailedMessage);
            return Ok(MessageConstant.Machinery.UpdateStatusSuccessMessage);

        }


    }
}
