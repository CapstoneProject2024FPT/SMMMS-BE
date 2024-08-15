using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Rank;
using SAM.BusinessTier.Services.Interfaces;
using SAM.DataTier.Models;
using SAM.DataTier.Paginate;
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
        public RankService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<RankService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewRank(CreateNewRankRequest createNewRankRequest)
        {
            
            Rank rank = await _unitOfWork.GetRepository<Rank>().SingleOrDefaultAsync(
                predicate: x => x.Name.Equals(createNewRankRequest.Name));
            if (rank != null) throw new BadHttpRequestException(MessageConstant.Rank.RankNameExisted);
            rank = _mapper.Map<Rank>(createNewRankRequest);
            rank.Id = Guid.NewGuid();

            await _unitOfWork.GetRepository<Rank>().InsertAsync(rank);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new BadHttpRequestException(MessageConstant.Rank.CreateNewRankFailedMessage);
            return rank.Id;
        }

        public async Task<IPaginate<GetAccountInforInRankResponse>> GetAccountInforInRank(Guid id, PagingModel pagingModel)
        {

            // Retrieve the rank or throw an exception if not found
            Rank rank = await _unitOfWork.GetRepository<Rank>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Rank.RankNotFoundMessage);

            // Retrieve the account IDs associated with the rank
            List<Guid> accountIds = (List<Guid>)await _unitOfWork.GetRepository<AccountRank>().GetListAsync(
                selector: x => x.AccountId,
                predicate: x => x.RankId.Equals(id));

            if (accountIds == null || accountIds.Count == 0)
            {
                throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);
            }

            // Retrieve the account information and map them to GetAccountInforInRankResponse with pagination
            IPaginate<GetAccountInforInRankResponse> response = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
                selector: x => _mapper.Map<GetAccountInforInRankResponse>(x),
                predicate: x => accountIds.Contains(x.Id),
                page: pagingModel.page,
                size: pagingModel.size,
                orderBy: x => x.OrderBy(x => x.Point));

            return response;
        }

        public async Task<GetRankResponse> GetRankById(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Rank.EmptyRankIdMessage);
            Rank rank = await _unitOfWork.GetRepository<Rank>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Rank.RankNotFoundMessage);
            return _mapper.Map<GetRankResponse>(rank);
        }

        public async Task<ICollection<GetRankResponse>> GetRankList(RankFilter filter)
        {
            ICollection<GetRankResponse> respone = await _unitOfWork.GetRepository<Rank>().GetListAsync(
               selector: x => _mapper.Map<GetRankResponse>(x),
               filter: filter,
               orderBy: x => x.OrderBy(x => x.Range));
            return respone;
        }

        public Task<bool> RemoveRankStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateRank(Guid id, UpdateRankRequest updateRankRequest)
        {
            Rank rank = await _unitOfWork.GetRepository<Rank>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Rank.RankNotFoundMessage);

            rank.Name = string.IsNullOrEmpty(updateRankRequest.Name) ? rank.Name : updateRankRequest.Name;
            rank.Range = updateRankRequest.Range.HasValue ? updateRankRequest.Range.Value : rank.Range;
            rank.Value = updateRankRequest.Value.HasValue ? updateRankRequest.Value.Value : rank.Range;


            _unitOfWork.GetRepository<Rank>().UpdateAsync(rank);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
