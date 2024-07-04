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
    public class WardsService : BaseService<WardsService>, IWardsService
    {
        public WardsService(IUnitOfWork<SamContext> unitOfWork, ILogger<WardsService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewWards(CreateNewWardRequest createNewWardsRequest)
        {
            throw new NotImplementedException();
        }

        public Task<GetWardResponse> GetWardsById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetWardResponse>> GetWardsList(WardFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveWardsStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateWards(Guid id, UpdateWardRequest updateWardsRequest)
        {
            throw new NotImplementedException();
        }
    }
}
