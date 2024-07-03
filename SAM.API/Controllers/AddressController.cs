using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
