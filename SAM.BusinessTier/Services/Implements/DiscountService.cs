using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.City;
using SAM.BusinessTier.Payload.Discount;
using SAM.BusinessTier.Payload.Districts;
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
    public class DiscountService : BaseService<DiscountService>, IDiscountService
    {
        public DiscountService(IUnitOfWork<SamContext> unitOfWork, ILogger<DiscountService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewDiscounts(CreateNewDiscountRequest createNewDiscountRequest)
        {
            Discount discount = await _unitOfWork.GetRepository<Discount>().SingleOrDefaultAsync();
            if (discount != null) throw new BadHttpRequestException(MessageConstant.District.DistrictExistedMessage);
            discount = _mapper.Map<Discount>(createNewDiscountRequest);
            discount.Id = Guid.NewGuid();
            discount.Status = DiscountStatus.Active.GetDescriptionFromEnum();
            discount.CreateDate = DateTime.Now;


            await _unitOfWork.GetRepository<Discount>().InsertAsync(discount);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Discount.CreateNewDiscountFailedMessage);
            return discount.Id;
        }

        public async Task<GetDiscountResponse> GetDiscountById(Guid id)
        {
            var discount = await _unitOfWork.GetRepository<Discount>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Discount.DiscountNotFoundMessage);
                    
            return _mapper.Map<GetDiscountResponse>(discount);
        }

        public async Task<ICollection<GetDiscountResponse>> GetDiscountList(DiscountFilter filter)
        {
            var discount = await _unitOfWork.GetRepository<Discount>().GetListAsync(
                selector: x => _mapper.Map<GetDiscountResponse>(x),
                filter: filter)
                ?? throw new BadHttpRequestException(MessageConstant.Discount.DiscountNotFoundMessage);
            return discount;
        }

        public async Task<bool> RemoveDiscountStatus(Guid id)
        {
            var discount = await _unitOfWork.GetRepository<Discount>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Discount.DiscountNotFoundMessage);
            discount.Status = DiscountStatus.InActive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Discount>().UpdateAsync(discount);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

        public async Task<bool> UpdateDiscount(Guid id, UpdateDiscountRequest updateDiscountRequest)
        {
            var discount = await _unitOfWork.GetRepository<Discount>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Discount.DiscountNotFoundMessage);
            discount.Name = string.IsNullOrEmpty(updateDiscountRequest.Name) ? discount.Name : updateDiscountRequest.Name;
            discount.Value = updateDiscountRequest.Value.HasValue ? updateDiscountRequest.Value.Value : updateDiscountRequest.Value;
            
            if (!updateDiscountRequest.Status.HasValue && !updateDiscountRequest.Type.HasValue)
            {
                throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
            }
            else
            {
                discount.Status = updateDiscountRequest.Status.GetDescriptionFromEnum();
                discount.Type = updateDiscountRequest.Type.GetDescriptionFromEnum();
            }
            _unitOfWork.GetRepository<Discount>().UpdateAsync(discount);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }
    }
}
