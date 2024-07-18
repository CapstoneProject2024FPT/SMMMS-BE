using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Order;
using SAM.BusinessTier.Payload.Task;
using SAM.BusinessTier.Payload.Warranty;
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

        public async Task<ICollection<GetWarrantyDetailResponse>> GetWarrantyDetailList(WarrantyDetailFilter filter)
        {
            var warrantyDetails = await _unitOfWork.GetRepository<WarrantyDetail>()
                .GetListAsync(
                    selector: detail => new GetWarrantyDetailResponse
                    {
                        Id = detail.Id,
                        Type = detail.Type != null ? EnumUtil.ParseEnum<WarrantyDetailType>(detail.Type) : null,
                        Status = detail.Status != null ? EnumUtil.ParseEnum<WarrantyDetailStatus>(detail.Status) : null,
                        CreateDate = detail.CreateDate,
                        StartDate = detail.StartDate,
                        CompletionDate = detail.CompletionDate,
                        Description = detail.Description,
                        Comments = detail.Comments,
                        NextMaintenanceDate = detail.NextMaintenanceDate,
                        UserInfor = detail.Account != null ? new OrderUserResponse
                        {
                            Id = detail.Account.Id,
                            FullName = detail.Account.FullName,
                            Role = EnumUtil.ParseEnum<RoleEnum>(detail.Account.Role)
                        } : null

                    },
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.StartDate),
                    include: x => x.Include(x => x.Account) // Include related user information
                ) ?? throw new BadHttpRequestException(MessageConstant.WarrantyDetail.WarrantyDetailNotFoundMessage);

            return warrantyDetails;
        }

        public async Task<GetWarrantyDetailResponse> GetWarrantyDetailById(Guid id)
        {
            var warrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>()
                .SingleOrDefaultAsync(
                    selector: detail => new GetWarrantyDetailResponse
                    {
                        Id = detail.Id,
                        Type = detail.Type != null ? EnumUtil.ParseEnum<WarrantyDetailType>(detail.Type) : null,
                        CreateDate = detail.CreateDate,
                        StartDate = detail.StartDate,
                        CompletionDate = detail.CompletionDate,
                        Status = detail.Status != null ? EnumUtil.ParseEnum<WarrantyDetailStatus>(detail.Status) : null,
                        Description = detail.Description,
                        Comments = detail.Comments,
                        NextMaintenanceDate = detail.NextMaintenanceDate,
                        UserInfor = detail.Account != null ? new OrderUserResponse
                        {
                            Id = detail.Account.Id,
                            FullName = detail.Account.FullName,
                            Role = EnumUtil.ParseEnum<RoleEnum>(detail.Account.Role)
                        } : null
                    },
                    predicate: detail => detail.Id == id,
                    include: x => x.Include(x => x.Account)
                );

            if (warrantyDetail == null)
            {
                throw new BadHttpRequestException($"WarrantyDetail with Id {id} not found.");
            }

            return warrantyDetail;
        }



        public Task<bool> RemoveWarrantyDetailStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWarrantyDetail(Guid id, UpdateWarrantyDetailRequest updateDetailRequest)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.WarrantyDetail.EmptyWarrantyDetailIdMessage);
            DateTime currentTime = TimeUtils.GetCurrentSEATime();
            WarrantyDetail warrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.WarrantyDetail.WarrantyDetailNotFoundMessage);
            Inventory inventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateDetailRequest.InventoryId))
            ?? throw new BadHttpRequestException(MessageConstant.Inventory.NotFoundFailedMessage);
            
            warrantyDetail.Status = updateDetailRequest.Status.GetDescriptionFromEnum();
            warrantyDetail.Description = !string.IsNullOrEmpty(updateDetailRequest.Description) ? updateDetailRequest.Description : warrantyDetail.Description;
            warrantyDetail.Comments = !string.IsNullOrEmpty(updateDetailRequest.Comments) ? updateDetailRequest.Comments : warrantyDetail.Comments;
            warrantyDetail.NextMaintenanceDate = updateDetailRequest.NextMaintenanceDate.HasValue ? updateDetailRequest.NextMaintenanceDate.Value : warrantyDetail.NextMaintenanceDate;
            
            if (updateDetailRequest.AccountId.HasValue)
            {
                Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: x => x.Id.Equals(updateDetailRequest.AccountId.Value))
                ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);

                warrantyDetail.Account = account;
            }
            if (updateDetailRequest.Status.GetDescriptionFromEnum() == "Completed")
            {
                warrantyDetail.CompletionDate = currentTime;
                var taskManager = await _unitOfWork.GetRepository<TaskManager>().SingleOrDefaultAsync(
                    predicate: t => t.WarrantyDetailId == warrantyDetail.Id);
                
                if (taskManager != null)
                {
                    taskManager.Status = TaskManagerStatus.Completed.GetDescriptionFromEnum();
                    _unitOfWork.GetRepository<TaskManager>().UpdateAsync(taskManager);
                }
            }
            _unitOfWork.GetRepository<WarrantyDetail>().UpdateAsync(warrantyDetail);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

    }
}
