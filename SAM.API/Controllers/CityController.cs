using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.City;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class CityController : BaseController<CityController>
    {
        readonly ICityService _cityService;

        public CityController(ILogger<CityController> logger, ICityService cityService) : base(logger)
        {
            _cityService = cityService;
        }
        [HttpPost(ApiEndPointConstant.City.CitiesEndPoint)]
        public async Task<IActionResult> CreateNewCity(CreateNewCityRequest createNewCityRequest)
        {
            var response = await _cityService.CreateNewCity(createNewCityRequest);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.City.CitiesEndPoint)]
        public async Task<IActionResult> GetCityList([FromQuery] CityFilter filter)
        {
            var response = await _cityService.GetCityList(filter);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.City.CityEndPoint)]
        public async Task<IActionResult> GetCityById(Guid id)
        {
            var response = await _cityService.GetCityById(id);
            return Ok(response);
        }

        [HttpPut(ApiEndPointConstant.City.CityEndPoint)]
        public async Task<IActionResult> UpdateCity(Guid id, UpdateCityRequest updateCityRequest)
        {
            var isSuccessful = await _cityService.UpdateCity(id, updateCityRequest);
            if (!isSuccessful) return Ok(MessageConstant.City.UpdateCityFailedMessage);
            return Ok(MessageConstant.City.UpdateCitySuccessMessage);
        }

        [HttpDelete(ApiEndPointConstant.City.CityEndPoint)]
        public async Task<IActionResult> RemoveCityStatus(Guid id)
        {
            var isSuccessful = await _cityService.RemoveCityStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.City.UpdateCityFailedMessage);
            return Ok(MessageConstant.City.UpdateCitySuccessMessage);
        }
    }
}
