﻿using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.Device;
using SAM.BusinessTier.Payload.Notification;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class DeviceController : BaseController<DeviceController>
    {
        readonly IDeviceService _deviceService;

        public DeviceController(ILogger<DeviceController> logger, IDeviceService deviceService) : base(logger)
        {
            _deviceService = deviceService;
        }
        [HttpPost(ApiEndPointConstant.Device.DevicesEndPoint)]
        public async Task<IActionResult> RegisterDevice(DeviceRegistrationRequest deviceRegistrationRequest)
        {
            var response = await _deviceService.RegisterDevice(deviceRegistrationRequest);
            return Ok(response);
        }

        [HttpDelete(ApiEndPointConstant.Device.DevicesEndPoint)]
        public async Task<IActionResult> RemoveDevice(string token)
        {
            var isSuccessful = await _deviceService.RemoveDevice(token);
            if (!isSuccessful) return Ok(MessageConstant.Device.UpdateStatusFailedMessage);
            return Ok(MessageConstant.Device.UpdateStatusSuccessMessage);
        }
    }
}
