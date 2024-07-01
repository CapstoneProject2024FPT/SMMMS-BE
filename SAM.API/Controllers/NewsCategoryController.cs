using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.NewsCategory;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class NewsCategoryController
        : BaseController<NewsCategoryController>
    {
        readonly INewsCategoryService _newsCategoryService;

        public NewsCategoryController(ILogger<NewsCategoryController> logger, INewsCategoryService newsCategoryService) : base(logger)
        {
            _newsCategoryService = newsCategoryService;
        }
        [HttpPost(ApiEndPointConstant.NewsCategory.NewsCategoriesEndPoint)]
        public async Task<IActionResult> CreateNewNewsCategory(CreateNewNewsCategoryRequest category)
        {
            var response = await _newsCategoryService.CreateNewNewsCategory(category);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.NewsCategory.NewsCategoriesEndPoint)]
        public async Task<IActionResult> GetNewsCategoryList([FromQuery] NewsCategoryFilter filter)
        {
            var response = await _newsCategoryService.GetNewsCategories(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.NewsCategory.NewsCategoryEndPoint)]
        public async Task<IActionResult> GetNewsCategory(Guid id)
        {
            var response = await _newsCategoryService.GetNewsCategory(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.NewsCategory.NewsCategoryEndPoint)]
        public async Task<IActionResult> UpdateCategory(Guid id, UpdateNewsCategoryRequest updateCategoryRequest)
        {
            var isSuccessful = await _newsCategoryService.UpdateNewsCategory(id, updateCategoryRequest);
            if (!isSuccessful) return Ok(MessageConstant.Category.UpdateCategoryFailedMessage);
            return Ok(MessageConstant.Category.UpdateCategorySuccessMessage);
        }
        [HttpDelete(ApiEndPointConstant.NewsCategory.NewsCategoryEndPoint)]
        public async Task<IActionResult> RemoveCategoryStatus(Guid id)
        {
            var isSuccessful = await _newsCategoryService.RemoveNewsCategoryStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Category.UpdateCategoryFailedMessage);
            return Ok(MessageConstant.Category.UpdateCategorySuccessMessage);
        }
    }
}
