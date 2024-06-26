using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.WarrantyDetail;
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
    public class WarrantyDetailService : BaseService<WarrantyDetailService>, IWarrantyDetailService
    {
        public WarrantyDetailService(IUnitOfWork<SamContext> unitOfWork, ILogger<WarrantyDetailService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewWarrantyDetail(CreateNewWarrantyDetailRequest createNewWarrantyDetailRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetWarrantyDetailResponse>> GetRankList(WarrantyDetailFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<GetBrandResponse> GetWarrantyDetailById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveWarrantyDetailStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateWarrantyDetail(Guid id, UpdateWarrantyDetailRequest updateWarrantyDetailRequest)
        {
            throw new NotImplementedException();
        }
    }
}
