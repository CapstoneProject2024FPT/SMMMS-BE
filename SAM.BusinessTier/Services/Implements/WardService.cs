using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Wards;
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
    public class WardService : BaseService<WardService>, IWardService
    {
        public WardService(IUnitOfWork<SamContext> unitOfWork, ILogger<WardService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewWard(CreateNewWardRequest createNewWardsRequest)
        {
            throw new NotImplementedException();
        }

        public Task<GetWardResponse> GetWardById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetWardResponse>> GetWardList(WardFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveWardStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateWard(Guid id, UpdateWardRequest updateWardsRequest)
        {
            throw new NotImplementedException();
        }
    }
}
