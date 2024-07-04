using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Discount;
using SAM.BusinessTier.Payload.Districts;
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
    public class DistrictsService : BaseService<DiscountService>, IDistrictsService
    {
        public DistrictsService(IUnitOfWork<SamContext> unitOfWork, ILogger<DiscountService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewDistricts(CreateNewDistrictRequest createNewDistrictRequest)
        {
            throw new NotImplementedException();
        }

        public Task<GetDistricResponse> GetDistrictsById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetDistricResponse>> GetDistrictsList(DistrictFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveDistrictsStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDistrict(Guid id, UpdateDistrictRequest updateDistrictsRequest)
        {
            throw new NotImplementedException();
        }
    }
}
