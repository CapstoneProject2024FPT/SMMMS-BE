using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload;
using SAM.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAM.BusinessTier.Payload.Product;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IProductService
    {
        Task<Guid> CreateNewProducts(CreateNewProductRequest createNewProductRequest);
        Task<bool> UpdateProduct(Guid id, UpdateProductRequest updateProductRequest);
        Task<IPaginate<GetProductsResponse>> GetProductList(ProductFilter filter, PagingModel pagingModel);
        Task<ICollection<GetProductsResponse>> GetProductListNotIPaginate(ProductFilter filter);
        Task<GetProductsResponse> GetProductById(Guid id);
        Task<bool> RemoveProductStatus(Guid id);
    }
}
