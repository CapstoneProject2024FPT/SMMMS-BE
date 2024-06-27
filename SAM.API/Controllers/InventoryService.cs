using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Inventory;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class InventoryService : BaseController<InventoryService>
    {
        readonly IInventoryService _inventoryService;

        public InventoryService(ILogger<InventoryService> logger, IInventoryService inventoryService) : base(logger)
        {
            _inventoryService = inventoryService;
        }
        [HttpPost(ApiEndPointConstant.Inventory.InventoriesEndPoint)]
        public async Task<IActionResult> CreateNewInventory(CreateNewInventoryRequest createNewInventoryRequest)
        {
            var response = await _inventoryService.CreateNewInventory(createNewInventoryRequest);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Inventory.InventoriesEndPoint)]
        public async Task<IActionResult> GetInventoryList([FromQuery] InventoryFilter filter)
        {
            var response = await _inventoryService.GetInventoryList(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Inventory.InventoryEndPoint)]
        public async Task<IActionResult> GetInventoryById(Guid id)
        {
            var response = await _inventoryService.GetInventoryById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Inventory.InventoryEndPoint)]
        public async Task<IActionResult> UpdateInventory(Guid id, UpdateInventoryRequest updateInventoryRequest)
        {
            var isSuccessful = await _inventoryService.UpdateInventory(id, updateInventoryRequest);
            if (!isSuccessful) return Ok(MessageConstant.Inventory.UpdateInventoryFailedMessage);
            return Ok(MessageConstant.Inventory.UpdateInventorySuccessMessage);
        }

        [HttpDelete(ApiEndPointConstant.Inventory.InventoryEndPoint)]
        public async Task<IActionResult> SwitchInventoryStatus(Guid id)
        {
            var isSuccessful = await _inventoryService.SwitchInventoryStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Inventory.UpdateInventoryFailedMessage);
            return Ok(MessageConstant.Inventory.UpdateInventorySuccessMessage);
        }
    }
}
