using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Discount;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class DiscountController : BaseController<DiscountController>
    {
        private readonly IDiscountService _discountService;

        public DiscountController(ILogger<DiscountController> logger, IDiscountService discountService) : base(logger)
        {
            _discountService = discountService;
        }
        [HttpPost(ApiEndPointConstant.Discount.AddCategoryToDiscount)]
        public async Task<IActionResult> AddDiscountToCategories(Guid id, [FromBody] List<Guid> categories)
        {
            var response = await _discountService.AddDiscountToCategories(id, categories);
            return Ok(response);
        }
        [HttpPost(ApiEndPointConstant.Discount.DiscountsEndPoint)]
        public async Task<IActionResult> CreateNewDiscounts(CreateNewDiscountRequest createNewDiscountRequest)
        {
            var response = await _discountService.CreateNewDiscounts(createNewDiscountRequest);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Discount.DiscountsEndPoint)]
        public async Task<IActionResult> GetDiscountList([FromQuery] DiscountFilter filter)
        {
            var response = await _discountService.GetDiscountList(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Discount.DiscountEndPoint)]
        public async Task<IActionResult> GetDiscountById(Guid id)
        {
            var response = await _discountService.GetDiscountById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Discount.DiscountEndPoint)]
        public async Task<IActionResult> UpdateDiscount(Guid id, UpdateDiscountRequest updateDiscountRequest)
        {
            var isSuccessful = await _discountService.UpdateDiscount(id, updateDiscountRequest);
            if (!isSuccessful) return Ok(MessageConstant.Discount.UpdateDiscountFailedMessage);
            return Ok(MessageConstant.Discount.UpdateDiscountSuccessMessage);
        }
        [HttpDelete(ApiEndPointConstant.Discount.DiscountEndPoint)]
        public async Task<IActionResult> RemoveDiscountStatus(Guid id)
        {
            var isSuccessful = await _discountService.RemoveDiscountStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Discount.UpdateDiscountFailedMessage);
            return Ok(MessageConstant.Discount.UpdateDiscountSuccessMessage);
        }
    }
}
