using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Rank;
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
    public class RankService : BaseService<RankService>, IRankService
    {
        public RankService(IUnitOfWork<SamContext> unitOfWork, ILogger<RankService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewRank(CreateNewRankRequest createNewRankRequest)
        {
            throw new NotImplementedException();
        }

        public Task<GetBrandResponse> GetRankById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetRankResponse>> GetRankList(RankFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveRankStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRank(Guid id, UpdateRankRequest updateRankRequest)
        {
            throw new NotImplementedException();
        }
    }
}
