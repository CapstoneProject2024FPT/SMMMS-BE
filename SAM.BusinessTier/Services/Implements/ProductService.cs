using AutoMapper;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.Product;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Paginate;
using SAM.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace SAM.BusinessTier.Services.Implements
{
    public class ProductService : BaseService<ProductService>, IProductService
    {
        public ProductService(IUnitOfWork<SamContext> unitOfWork, ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewProducts(CreateNewProductRequest createNewProductRequest)
        {
            Product product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: x => x.Name.Equals(createNewProductRequest.Name));
            if (product != null) throw new BadHttpRequestException(MessageConstant.Product.ProductNameExisted);
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(createNewProductRequest.CategoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
            product = _mapper.Map<Product>(createNewProductRequest);
            await _unitOfWork.GetRepository<Product>().InsertAsync(product);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new BadHttpRequestException(MessageConstant.Product.CreateNewProductFailedMessage);
            return product.Id;
        }

        public async Task<GetProductsResponse> GetProductById(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.EmptyProductIdMessage);
            Product product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
            return _mapper.Map<GetProductsResponse>(product);
        }

        public async Task<IPaginate<GetProductsResponse>> GetProductList(ProductFilter filter, PagingModel pagingModel)
        {
            IPaginate<GetProductsResponse> respone = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
               selector: x => _mapper.Map<GetProductsResponse>(x),
               filter: filter,
               page: pagingModel.page,
               size: pagingModel.size
/*               orderBy: x => x.OrderBy(x => x.Priority)*/);
            return respone;
        }

        public async Task<ICollection<GetProductsResponse>> GetProductListNotIPaginate(ProductFilter filter)
        {
            ICollection<GetProductsResponse> respone = await _unitOfWork.GetRepository<Product>().GetListAsync(
               selector: x => _mapper.Map<GetProductsResponse>(x),
               filter: filter
/*               orderBy: x => x.OrderBy(x => x.Priority)*/);
            return respone;
        }

        public async Task<bool> RemoveProductStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.EmptyProductIdMessage);
            Product product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Product.ProductNotFoundMessage);
            product.Status = ProductStatus.Inactive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Product>().UpdateAsync(product);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateProduct(Guid id, UpdateProductRequest updateProductRequest)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.EmptyProductIdMessage);
            Product product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Product.ProductNameExisted);
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateProductRequest.CategoryId))
            ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
            
            product.Name = string.IsNullOrEmpty(updateProductRequest.Name) ? product.Name : updateProductRequest.Name;
            product.StockPrice = updateProductRequest.StockPrice;
            product.Description = string.IsNullOrEmpty(updateProductRequest.Description) ? product.Description : updateProductRequest.Description;
            product.Status = updateProductRequest.Status.GetDescriptionFromEnum();
            //product.Priority = updateProductRequest.Priority;
            _unitOfWork.GetRepository<Product>().UpdateAsync(product);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }
    }
}
