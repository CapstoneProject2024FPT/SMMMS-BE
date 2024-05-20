using DentalLabManagement.API.Controllers;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Category;

using SAM.BusinessTier.Services.Implements;

using Microsoft.AspNetCore.Mvc;

namespace SAM.API.Controllers
{
    [ApiController]
    public class CategoryController : BaseController<CategoryController>
        
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService) : base(logger)
        {
            _categoryService = categoryService;
        }
        [HttpPost(ApiEndPointConstant.Category.CategoriesEndPoint)]
        public async Task<IActionResult> CreateNewCategory (CreateNewCategoryRequest category)
        {
            var response = await _categoryService.CreateNewCategory(category);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Category.CategoriesEndPoint)]
        public async Task<IActionResult> GetAllCategories([FromQuery] CategoryFilter filter, [FromQuery] PagingModel pagingModel)
        {
            var response = await _categoryService.GetCategories(filter, pagingModel);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Category.CategoryEndPoint)]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var response = await _categoryService.GetCategory(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Category.CategoryEndPoint)]
        public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryRequest updateCategoryRequest)
        {
            var isSuccessful = await _categoryService.UpdateCategory(id, updateCategoryRequest);
            if (!isSuccessful) return Ok(MessageConstant.Category.UpdateCategoryFailedMessage);
            return Ok(MessageConstant.Category.UpdateCategorySuccessMessage);
        }
        [HttpDelete(ApiEndPointConstant.Category.CategoryEndPoint)]
        public async Task<IActionResult> RemoveCategoryStatus(Guid id)
        {
            var isSuccessful = await _categoryService.RemoveCategoryStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Category.UpdateCategoryFailedMessage);
            return Ok(MessageConstant.Category.UpdateCategorySuccessMessage);
        }

    }
}
