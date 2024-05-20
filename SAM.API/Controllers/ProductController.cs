using DentalLabManagement.API.Controllers;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.Product;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;
using SAM.DataTier.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SAM.API.Controllers
{
    [ApiController]
    public class ProductController : BaseController<ProductController>
    {
        private readonly IProductService _iProductService;

        public ProductController(ILogger<ProductController> logger, IProductService productService) : base(logger)
        {
            _iProductService = productService;
        }
        [HttpPost(ApiEndPointConstant.Product.ProductsEndPoint)]
        public async Task<IActionResult> CreateNewProducts(CreateNewProductRequest product)
        {
            var response = await _iProductService.CreateNewProducts(product);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Product.ProductsEndPoint)]
        public async Task<IActionResult> GetProductList([FromQuery] ProductFilter filter, [FromQuery] PagingModel pagingModel)
        {
            var response = await _iProductService.GetProductList(filter, pagingModel);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Product.ProductsEndPointNoPaginate)]
        public async Task<IActionResult> GetProductListNotIPaginate([FromQuery] ProductFilter filter)
        {
            var response = await _iProductService.GetProductListNotIPaginate(filter);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Product.ProductEndPoint)]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var response = await _iProductService.GetProductById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Product.ProductEndPoint)]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductRequest updateProductRequest)
        {
            var response = await _iProductService.UpdateProduct(id, updateProductRequest);
            if (!response) return Ok(MessageConstant.Product.UpdateProductFailedMessage);
            return Ok(MessageConstant.Product.UpdateStatusSuccessMessage);
        }
        [HttpDelete(ApiEndPointConstant.Product.ProductEndPoint)]
        public async Task<IActionResult> RemoveProductStatus(Guid id)
        {
            var response = await _iProductService.RemoveProductStatus(id);
            if (!response) return Ok(MessageConstant.Product.UpdateProductFailedMessage);
            return Ok(MessageConstant.Product.UpdateStatusSuccessMessage);

        }
    }
}
