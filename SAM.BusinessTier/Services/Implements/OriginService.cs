using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Origin;
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
    public class OriginService : BaseService<OriginService>, IOriginService
    {
        public OriginService(IUnitOfWork<SamContext> unitOfWork, ILogger<OriginService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewOrigin(CreateNewOriginRequest createNewOriginRequest)
        {
            throw new NotImplementedException();
        }

        public Task<GetOriginResponse> GetOriginById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetOriginResponse>> GetOriginList(OriginFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveOriginStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateOrigin(Guid id, UpdateOriginRequest updateOriginRequest)
        {
            throw new NotImplementedException();
        }
    }
}
