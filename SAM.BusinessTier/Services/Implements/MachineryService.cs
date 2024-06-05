using AutoMapper;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Paginate;
using SAM.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Machinery;


namespace SAM.BusinessTier.Services.Implements
{
    public class MachineryService : BaseService<MachineryService>, IMachineryService
    {
        public MachineryService(IUnitOfWork<SamContext> unitOfWork, ILogger<MachineryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewMachinerys(CreateNewMachineryRequest createNewMachineryRequest)
        {
            Machinery machinery = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                predicate: x => x.Name.Equals(createNewMachineryRequest.Name));
            if (machinery != null) throw new BadHttpRequestException(MessageConstant.Product.ProductNameExisted);
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(createNewMachineryRequest.CategoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
            machinery = _mapper.Map<Machinery>(createNewMachineryRequest);
            await _unitOfWork.GetRepository<Machinery>().InsertAsync(machinery);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new BadHttpRequestException(MessageConstant.Product.CreateNewProductFailedMessage);
            return machinery.Id;
        }

        public async Task<GetMachinerysResponse> GetMachineryById(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.EmptyProductIdMessage);
            Machinery product = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
            return _mapper.Map<GetMachinerysResponse>(product);
        }

        public async Task<IPaginate<GetMachinerysResponse>> GetMachineryList(MachineryFilter filter, PagingModel pagingModel)
        {
            IPaginate<GetMachinerysResponse> respone = await _unitOfWork.GetRepository<Machinery>().GetPagingListAsync(
               selector: x => _mapper.Map<GetMachinerysResponse>(x),
               filter: filter,
               page: pagingModel.page,
               size: pagingModel.size
/*               orderBy: x => x.OrderBy(x => x.Priority)*/);
            return respone;
        }


        public async Task<ICollection<GetMachinerysResponse>> GetMachineryListNotIPaginate(MachineryFilter filter)
        {
            ICollection<GetMachinerysResponse> respone = await _unitOfWork.GetRepository<Machinery>().GetListAsync(
               selector: x => _mapper.Map<GetMachinerysResponse>(x),
               filter: filter
/*               orderBy: x => x.OrderBy(x => x.Priority)*/);
            return respone;
        }


        public async Task<bool> RemoveMachineryStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.EmptyProductIdMessage);
            Machinery product = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Product.ProductNotFoundMessage);
            product.Status = ProductStatus.Inactive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Machinery>().UpdateAsync(product);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateMachinery(Guid id, UpdateMachineryRequest updateProductRequest)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.EmptyProductIdMessage);
            Machinery product = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
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
            _unitOfWork.GetRepository<Machinery>().UpdateAsync(product);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }


    }
}
