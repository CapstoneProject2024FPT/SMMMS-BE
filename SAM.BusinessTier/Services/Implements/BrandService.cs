using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Implements
{
    public class BrandService : BaseService<BrandService>, IBrandService
    {
        public BrandService(IUnitOfWork<SamContext> unitOfWork, ILogger<BrandService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewBrand(CreateNewBrandRequest request)
        {

            Brand brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(
                predicate: x => x.Name.Equals(request.Name));
            if (brand != null) throw new BadHttpRequestException(MessageConstant.Brand.BrandExistedMessage);
            brand = _mapper.Map<Brand>(request);
            await _unitOfWork.GetRepository<Brand>().InsertAsync(brand);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new BadHttpRequestException(MessageConstant.Brand.CreateBrandFailedMessage);
            return brand.Id;
        }

        public async Task<GetBrandResponse> GetBrandById(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.BrandEmptyMessage);
            Brand brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Brand.NotFoundFailedMessage);
            return _mapper.Map<GetBrandResponse>(brand);
        }

        public async Task<ICollection<GetBrandResponse>> GetBrandList(BrandFilter filter)
        {
            ICollection<GetBrandResponse> respone = await _unitOfWork.GetRepository<Brand>().GetListAsync(
               selector: x => _mapper.Map<GetBrandResponse>(x),
               filter: filter);
            return respone;
        }

        public async Task<bool> RemoveBrandStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.BrandEmptyMessage);
            Brand brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Brand.NotFoundFailedMessage);
            brand.Status = BrandStatus.Inactive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Brand>().UpdateAsync(brand);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateBrand(Guid id, UpdateBrandRequest updateBrandRequest)
        {
            Brand brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Brand.NotFoundFailedMessage);

            brand.Name = string.IsNullOrEmpty(updateBrandRequest.Name) ? brand.Name : updateBrandRequest.Name;
            brand.Description = string.IsNullOrEmpty(updateBrandRequest.Description) ? brand.Description : updateBrandRequest.Description;
            brand.Status = updateBrandRequest.Status.GetDescriptionFromEnum();

           
            _unitOfWork.GetRepository<Brand>().UpdateAsync(brand);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
