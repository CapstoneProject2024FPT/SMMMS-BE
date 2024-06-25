using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class BrandController : BaseController<BrandController>  
    {
        readonly IBrandService _brandService;
        public BrandController(ILogger<BrandController> logger, IBrandService brandService) : base(logger)
        {
            _brandService = brandService;
        }
        [HttpPost(ApiEndPointConstant.Brand.BrandsEndPoint)]
        public async Task<IActionResult> CreateNewBrand(CreateNewBrandRequest createNewBrandRequest)
        {
            var response = await _brandService.CreateNewBrand(createNewBrandRequest);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Brand.BrandsEndPoint)]
        public async Task<IActionResult> GetBrandList([FromQuery] BrandFilter filter)
        {
            var response = await _brandService.GetBrandList(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Brand.BrandEndPoint)]
        public async Task<IActionResult> GetBrandById(Guid id)
        {
            var response = await _brandService.GetBrandById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Brand.BrandEndPoint)]
        public async Task<IActionResult> UpdateBrand(Guid id, UpdateBrandRequest updateBrandRequest)
        {
            var isSuccessful = await _brandService.UpdateBrand(id, updateBrandRequest);
            if (!isSuccessful) return Ok(MessageConstant.Brand.UpdateBrandFailedMessage);
            return Ok(MessageConstant.Brand.UpdateBrandSuccessMessage);
        }
        [HttpDelete(ApiEndPointConstant.Brand.BrandEndPoint)]
        public async Task<IActionResult> RemoveBrandStatus(Guid id)
        {
            var isSuccessful = await _brandService.RemoveBrandStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Brand.UpdateBrandFailedMessage);
            return Ok(MessageConstant.Brand.UpdateBrandSuccessMessage);
        }
    }
}
