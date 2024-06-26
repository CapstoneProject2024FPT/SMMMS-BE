using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Warranty;
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
    public class WarrantyService : BaseService<WarrantyService>, IWarrantyService
    {
        public WarrantyService(IUnitOfWork<SamContext> unitOfWork, ILogger<WarrantyService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewWarranty(CreateNewWarrantyRequest createNewWarrantyRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetWarrantyInforResponse>> GetRankList(WarrantyFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<GetWarrantyInforResponse> GetWarrantyById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveWarrantyStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateWarranty(Guid id, UpdateWarrantyRequest updateWarrantyRequest)
        {
            throw new NotImplementedException();
        }
    }
}
