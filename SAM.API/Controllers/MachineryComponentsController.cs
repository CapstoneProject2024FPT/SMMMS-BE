using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Discount;
using SAM.BusinessTier.Payload.MachineryComponent;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class MachineryComponentsController : BaseController<MachineryComponentsController>
    {
        private readonly IMachineryComponentService _machineryComponentService;
            public MachineryComponentsController(ILogger<MachineryComponentsController> logger, IMachineryComponentService machineryComponentService) : base(logger)
        {
            _machineryComponentService = machineryComponentService;
        }
        [HttpPost(ApiEndPointConstant.MachineryComponent.MachineryComponentsEndPoint)]
        public async Task<IActionResult> CreateNewMachineryComponents(CreateNewMachineryComponentRequest createNewMachineryComponentRequest)
        {
            var response = await _machineryComponentService.CreateNewMachineryComponents(createNewMachineryComponentRequest);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.MachineryComponent.MachineryComponentsEndPoint)]
        public async Task<IActionResult> GetMachineryComponentList([FromQuery] MachineryComponentFilter filter, [FromQuery] PagingModel pagingModel)
        {
            var response = await _machineryComponentService.GetMachineryComponentList(filter, pagingModel);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.MachineryComponent.MachineryComponentsEndPointNoPaginate)]
        public async Task<IActionResult> GetMachineryComponentListNoPagingNate([FromQuery] MachineryComponentFilter filter)
        {
            var response = await _machineryComponentService.GetMachineryComponentListNoPagingNate(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.MachineryComponent.MachineryComponentEndPoint)]
        public async Task<IActionResult> GetMachineryComponentById(Guid id)
        {
            var response = await _machineryComponentService.GetMachineryComponentById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.MachineryComponent.MachineryComponentEndPoint)]
        public async Task<IActionResult> UpdateMachineryComponent(Guid id, UpdateMachineryComponentRequest updateMachineryComponentRequest)
        {
            var isSuccessful = await _machineryComponentService.UpdateMachineryComponent(id, updateMachineryComponentRequest);
            if (!isSuccessful) return Ok(MessageConstant.MachineryComponents.UpdateMachineryComponentsFailedMessage);
            return Ok(MessageConstant.MachineryComponents.UpdateMachineryComponentsSuccessMessage);
        }
        [HttpDelete(ApiEndPointConstant.MachineryComponent.MachineryComponentEndPoint)]
        public async Task<IActionResult> RemoveMachineryComponentStatus(Guid id)
        {
            var isSuccessful = await _machineryComponentService.RemoveMachineryComponentStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.MachineryComponents.UpdateMachineryComponentsFailedMessage);
            return Ok(MessageConstant.MachineryComponents.UpdateMachineryComponentsSuccessMessage);
        }
    }
}
