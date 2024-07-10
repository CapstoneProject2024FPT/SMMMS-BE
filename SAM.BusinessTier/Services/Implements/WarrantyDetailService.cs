using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.WarrantyDetail;
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
    public class WarrantyDetailService : BaseService<WarrantyDetailService>, IWarrantyDetailService
    {
        public WarrantyDetailService(IUnitOfWork<SamContext> unitOfWork, ILogger<WarrantyDetailService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewWarrantyDetail(CreateNewWarrantyDetailRequest createNewWarrantyDetailRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetWarrantyDetailResponse>> GetWarrantyDetailList(WarrantyDetailFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<GetWarrantyDetailResponse> GetWarrantyDetailById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveWarrantyDetailStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWarrantyDetail(Guid id, UpdateWarrantyDetailRequest updateDetailRequest)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.WarrantyDetail.EmptyWarrantyDetailIdMessage);

            WarrantyDetail warrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.WarrantyDetail.WarrantyDetailNotFoundMessage);

            // Update warranty detail properties
            warrantyDetail.CompletionDate = updateDetailRequest.CompletionDate.HasValue ? updateDetailRequest.CompletionDate.Value : warrantyDetail.CompletionDate;
            warrantyDetail.Status = updateDetailRequest.Status.GetDescriptionFromEnum();
            warrantyDetail.Description = !string.IsNullOrEmpty(updateDetailRequest.Description) ? updateDetailRequest.Description : warrantyDetail.Description;
            warrantyDetail.Comments = !string.IsNullOrEmpty(updateDetailRequest.Comments) ? updateDetailRequest.Comments : warrantyDetail.Comments;
            warrantyDetail.NextMaintenanceDate = updateDetailRequest.NextMaintenanceDate.HasValue ? updateDetailRequest.NextMaintenanceDate.Value : warrantyDetail.NextMaintenanceDate;

            // Update associated account if AccountId is provided
            if (updateDetailRequest.AccountId.HasValue)
            {
                Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: x => x.Id.Equals(updateDetailRequest.AccountId.Value))
                ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);

                warrantyDetail.Account = account;
            }

            _unitOfWork.GetRepository<WarrantyDetail>().UpdateAsync(warrantyDetail);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

    }
}
