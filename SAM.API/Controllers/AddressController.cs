using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Address;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class AddressController : BaseController<AddressController>
    {
        readonly IAddressService _addressService;

        public AddressController(ILogger<AddressController> logger, IAddressService addressService) : base(logger)
        {
            _addressService = addressService;
        }
        [HttpPost(ApiEndPointConstant.Address.AddressSEndPoint)]
        public async Task<IActionResult> CreateNewAddress(CreateNewAddressRequest createNewAddresRequest)
        {
            var response = await _addressService.CreateNewAddress(createNewAddresRequest);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Address.AddressSEndPoint)]
        public async Task<IActionResult> GetAddressList([FromQuery] AddressFilter filter)
        {
            var response = await _addressService.GetAddressList(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Address.AddressEndPoint)]
        public async Task<IActionResult> GetAddressById(Guid id)
        {
            var response = await _addressService.GetAddressById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Address.AddressEndPoint)]
        public async Task<IActionResult> UpdateAddress(Guid id, UpdateAddressRequest updateAddressRequest)
        {
            var isSuccessful = await _addressService.UpdateAddress(id, updateAddressRequest);
            if (!isSuccessful) return Ok(MessageConstant.Address.UpdateAddressFailedMessage);
            return Ok(MessageConstant.Address.UpdateAddressSuccessMessage);
        }
        [HttpDelete(ApiEndPointConstant.Address.AddressEndPoint)]
        public async Task<IActionResult> RemoveAddressStatus(Guid id)
        {
            var isSuccessful = await _addressService.RemoveAddressStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Address.UpdateAddressFailedMessage);
            return Ok(MessageConstant.Address.UpdateAddressSuccessMessage);
        }
    }
}
