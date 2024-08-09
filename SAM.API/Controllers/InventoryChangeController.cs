using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class InventoryChangeController : BaseController<InventoryChangeController>
    {
        readonly IInventoryChangeService _inventoryChangeService;

        public InventoryChangeController(ILogger<InventoryChangeController> logger, IInventoryChangeService inventoryChangeService) : base(logger)
        {
            _inventoryChangeService = inventoryChangeService;
        }
        [HttpDelete(ApiEndPointConstant.InventoryChange.InventoryChangeEndPoint)]
        public async Task<IActionResult> DeleteInventoryChange(Guid id)
        {
            var isSuccessful = await _inventoryChangeService.DeleteInventoryChange(id);
            if (!isSuccessful) return Ok(MessageConstant.Inventory.DeleteInventoryFailedMessage);
            return Ok(MessageConstant.Inventory.DeleteInventorySuccessMessage);
        }
    }
}
