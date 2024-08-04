using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Wards;
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
    public class WardService : BaseService<WardService>, IWardService
    {
        public WardService(IUnitOfWork<SamContext> unitOfWork, ILogger<WardService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewWard(CreateNewWardRequest createNewWardRequest)
        {
            Ward ward = _mapper.Map<Ward>(createNewWardRequest);
            ward.Id = Guid.NewGuid();
            ward.Status = WardStatus.Active.GetDescriptionFromEnum();

            await _unitOfWork.GetRepository<Ward>().InsertAsync(ward);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Ward.CreateWardFailedMessage);

            return ward.Id;
        }


        public async Task<GetWardResponse> GetWardById(Guid id)
        {
            var ward = await _unitOfWork.GetRepository<Ward>()
                .SingleOrDefaultAsync(
                    predicate: x => x.Id == id,
                    include: x => x.Include(x => x.District))
                ?? throw new BadHttpRequestException(MessageConstant.Ward.NotFoundFailedMessage);

            var wardResponse = new GetWardResponse
            {
                Id = ward.Id,
                Name = ward.Name,
                Status = string.IsNullOrEmpty(ward.Status) ? null : EnumUtil.ParseEnum<WardStatus>(ward.Status),
                District = new DistrictResponse
                {
                    Id = ward.District.Id,
                    Name = ward.District.Name,
                    UnitId = ward.District.UnitId,
                },
            };
            return wardResponse;
        }

        public async Task<ICollection<GetWardResponse>> GetWardList(WardFilter filter)
        {
            var wardList = await _unitOfWork.GetRepository<Ward>()
                .GetListAsync(
                    selector: x => x,
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.Name),
                    include: x => x.Include(x => x.District))
                ?? throw new BadHttpRequestException(MessageConstant.Ward.NotFoundFailedMessage);

            var wardResponses = wardList.Select(ward => new GetWardResponse
            {
                Id = ward.Id,
                Name = ward.Name,
                Status = string.IsNullOrEmpty(ward.Status) ? null : EnumUtil.ParseEnum<WardStatus>(ward.Status),
                District = new DistrictResponse
                {
                    Id = ward.District.Id,
                    Name = ward.District.Name,
                    UnitId = ward.District.UnitId,
                },
            }).ToList();

            return wardResponses;
        }

        public async Task<bool> RemoveWardStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Ward.WardEmptyMessage);
            Ward ward = await _unitOfWork.GetRepository<Ward>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.News.NewsNotFoundMessage);
            ward.Status = WardStatus.InActive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Ward>().UpdateAsync(ward);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateWard(Guid id, UpdateWardRequest updateWardRequest)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException(MessageConstant.Ward.WardEmptyMessage);

            var ward = await _unitOfWork.GetRepository<Ward>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Ward.WardExistedMessage);

            ward.Name = string.IsNullOrEmpty(updateWardRequest.Name) ? ward.Name : updateWardRequest.Name;
            
            if (!updateWardRequest.Status.HasValue)
            {
                throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
            }
            else
            {
                ward.Status = updateWardRequest.Status.GetDescriptionFromEnum();
            }
            _unitOfWork.GetRepository<Ward>().UpdateAsync(ward);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }
    }
}
