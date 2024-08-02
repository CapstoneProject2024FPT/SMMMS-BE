﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Brand;
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

        public Task<GetBrandResponse> GetDiscountById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetDiscountResponse>> GetDiscountList(DiscountFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveDiscountStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDiscount(Guid id, UpdateDiscountRequest updateDiscountRequest)
        {
            throw new NotImplementedException();
        }

        Task<GetDiscountResponse> IDiscountService.GetDiscountById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
