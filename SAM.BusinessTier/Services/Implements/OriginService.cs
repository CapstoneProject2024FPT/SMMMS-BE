using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.News;
using SAM.BusinessTier.Payload.Origin;
using SAM.BusinessTier.Payload.Rank;
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
    public class OriginService : BaseService<OriginService>, IOriginService
    {
        public OriginService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<OriginService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewOrigin(CreateNewOriginRequest createNewOriginRequest)
        {
            Origin origin = await _unitOfWork.GetRepository<Origin>().SingleOrDefaultAsync(
                predicate: x => x.Name.Equals(createNewOriginRequest.Name));
            if (origin != null) throw new BadHttpRequestException(MessageConstant.Origin.OriginExistedMessage);
            origin = _mapper.Map<Origin>(createNewOriginRequest);
            await _unitOfWork.GetRepository<Origin>().InsertAsync(origin);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new BadHttpRequestException(MessageConstant.Origin.CreateOriginFailedMessage);
            return origin.Id;
        }

        public async Task<GetOriginResponse> GetOriginById(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Origin.OriginEmptyMessage);
            Origin origin = await _unitOfWork.GetRepository<Origin>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Origin.NotFoundFailedMessage);
            return _mapper.Map<GetOriginResponse>(origin);
        }

        public async Task<ICollection<GetOriginResponse>> GetOriginList(OriginFilter filter)
        {
            ICollection<GetOriginResponse> respone = await _unitOfWork.GetRepository<Origin>().GetListAsync(
               selector: x => _mapper.Map<GetOriginResponse>(x),
               filter: filter);
            return respone;
        }

        public async Task<bool> RemoveOriginStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Origin.OriginEmptyMessage);
            Origin origin = await _unitOfWork.GetRepository<Origin>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Origin.NotFoundFailedMessage);
            foreach (var item in origin.Machineries)
            {
                item.Status = MachineryStatus.UnAvailable.GetDescriptionFromEnum();
            }
            origin.Status = CategoryStatus.Inactive.GetDescriptionFromEnum();
            foreach (var item in origin.MachineComponents)
            {
                item.Status = ComponentStatus.InActive.GetDescriptionFromEnum();
            }
            origin.Status = OriginStatus.Inactive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Origin>().UpdateAsync(origin);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateOrigin(Guid id, UpdateOriginRequest updateOriginRequest)
        {
            Origin origin = await _unitOfWork.GetRepository<Origin>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Origin.NotFoundFailedMessage);

            origin.Name = string.IsNullOrEmpty(updateOriginRequest.Name) ? origin.Name : updateOriginRequest.Name;
            origin.Description = string.IsNullOrEmpty(updateOriginRequest.Description) ? origin.Description : updateOriginRequest.Description;
            
            if (!updateOriginRequest.Status.HasValue)
            {
                throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
            }
            else
            {
                origin.Status = updateOriginRequest.Status.GetDescriptionFromEnum();
            }
            switch (updateOriginRequest.Status)
            {
                case OriginStatus.Active:
                    foreach (var item in origin.Machineries)
                    {
                        item.Status = MachineryStatus.Available.GetDescriptionFromEnum();
                    }
                    foreach (var item in origin.MachineComponents)
                    {
                        item.Status = MachineryStatus.Available.GetDescriptionFromEnum();
                    }
                    break;
                case OriginStatus.Inactive:
                    foreach (var item in origin.Machineries)
                    {
                        item.Status = MachineryStatus.UnAvailable.GetDescriptionFromEnum();
                    }
                    foreach (var item in origin.MachineComponents)
                    {
                        item.Status = ComponentStatus.InActive.GetDescriptionFromEnum();
                    }

                    break;
                default:
                    return !true;
            }

            _unitOfWork.GetRepository<Origin>().UpdateAsync(origin);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
