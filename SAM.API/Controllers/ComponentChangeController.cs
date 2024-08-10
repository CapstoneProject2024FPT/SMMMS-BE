using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class ComponentChangeController : BaseController<ComponentChangeController>
    {
        readonly IComponentChangeService _inventoryChangeService;

        public ComponentChangeController(ILogger<ComponentChangeController> logger, IComponentChangeService inventoryChangeService) : base(logger)
        {
            _inventoryChangeService = inventoryChangeService;
        }
        [HttpDelete(ApiEndPointConstant.ComponentChange.ComponentChangeEndPoint)]
        public async Task<IActionResult> DeleteInventoryChange(Guid id)
        {
            var isSuccessful = await _inventoryChangeService.DeleteComponentChange(id);
            if (!isSuccessful) return Ok(MessageConstant.Inventory.DeleteInventoryFailedMessage);
            return Ok(MessageConstant.Inventory.DeleteInventorySuccessMessage);
        }
    }
}
