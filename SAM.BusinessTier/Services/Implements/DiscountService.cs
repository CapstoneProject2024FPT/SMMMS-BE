using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Discount;
using SAM.BusinessTier.Services.Interfaces;
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

        public Task<Guid> CreateNewDiscounts(CreateNewDiscountRequest createNewDiscountRequest)
        {
            throw new NotImplementedException();
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
