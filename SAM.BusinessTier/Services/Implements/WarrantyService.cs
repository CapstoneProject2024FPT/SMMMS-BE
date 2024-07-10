using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Warranty;
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
    public class WarrantyService : BaseService<WarrantyService>, IWarrantyService
    {
        public WarrantyService(IUnitOfWork<SamContext> unitOfWork, ILogger<WarrantyService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewWarranty(CreateNewWarrantyRequest createNewWarrantyRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<GetWarrantyInforResponse>> GetWarrantyList(WarrantyFilter filter)
        {
            var warrantyList = await _unitOfWork.GetRepository<Warranty>()
                .GetListAsync(
            selector: warranty => new GetWarrantyInforResponse
            {
                Id = warranty.Id,
                Type = EnumUtil.ParseEnum<WarrantyType>(warranty.Type),
                CreateDate = warranty.CreateDate,
                StartDate = warranty.StartDate,
                CompletionDate = warranty.CompletionDate,
                Status = EnumUtil.ParseEnum<WarrantyStatus>(warranty.Status),
                Description = warranty.Description,
                Comments = warranty.Comments,
                NextMaintenanceDate = warranty.NextMaintenanceDate,
                Priority = warranty.Priority,
                InventoryId = warranty.InventoryId,
                WarrantyDetail = warranty.WarrantyDetails.Select(detail => new WarrantyDetailResponse
                {
                    Id = detail.Id,
                    Status = EnumUtil.ParseEnum<WarrantyDetailStatus>(detail.Status),
                    CreateDate = detail.CreateDate,
                    StartDate = detail.StartDate,
                    Description = detail.Description,
                    Comments = detail.Comments,
                    WarrantyId = detail.WarrantyId,
                    AccountId = detail.AccountId
                }).ToList()
            },
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.Priority),
                    include: x => x.Include(x => x.WarrantyDetails)
                ) ?? throw new BadHttpRequestException(MessageConstant.Warranty.WarrantyNotFoundMessage);

            return warrantyList;
        }

        public async Task<GetWarrantyInforResponse> GetWarrantyById(Guid id)
        {
            var warranty = await _unitOfWork.GetRepository<Warranty>()
                .SingleOrDefaultAsync(
                    selector: warranty => new GetWarrantyInforResponse
                    {
                        Id = warranty.Id,
                        Type = EnumUtil.ParseEnum<WarrantyType>(warranty.Type),
                        CreateDate = warranty.CreateDate,
                        StartDate = warranty.StartDate,
                        CompletionDate = warranty.CompletionDate,
                        Status = EnumUtil.ParseEnum<WarrantyStatus>(warranty.Status),
                        Description = warranty.Description,
                        Comments = warranty.Comments,
                        NextMaintenanceDate = warranty.NextMaintenanceDate,
                        Priority = warranty.Priority,
                        InventoryId = warranty.InventoryId,
                        WarrantyDetail = warranty.WarrantyDetails.Select(detail => new WarrantyDetailResponse
                        {
                            Id = detail.Id,
                            Status = EnumUtil.ParseEnum<WarrantyDetailStatus>(detail.Status),
                            CreateDate = detail.CreateDate,
                            StartDate = detail.StartDate,
                            Description = detail.Description,
                            Comments = detail.Comments,
                            WarrantyId = detail.WarrantyId,
                            AccountId = detail.AccountId
                        }).ToList()
                    },
                    include: x => x.Include(x => x.WarrantyDetails)
                );

            if (warranty == null)
            {
                throw new BadHttpRequestException(MessageConstant.Warranty.WarrantyNotFoundMessage);
            }

            return warranty;
        }


        public Task<bool> RemoveWarrantyStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWarranty(Guid id, UpdateWarrantyRequest updateWarrantyRequest)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Warranty.EmptyWarrantyIdMessage);

            Warranty warranty = await _unitOfWork.GetRepository<Warranty>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Warranty.WarrantyNotFoundMessage);

            // Update warranty properties
            warranty.Status = updateWarrantyRequest.Status.GetDescriptionFromEnum(); ;
            warranty.Description = string.IsNullOrEmpty(updateWarrantyRequest.Description) ? updateWarrantyRequest.Description : warranty.Description;
            warranty.Comments = string.IsNullOrEmpty(updateWarrantyRequest.Comments) ? updateWarrantyRequest.Comments : warranty.Comments;
            warranty.NextMaintenanceDate = updateWarrantyRequest.NextMaintenanceDate.HasValue ? updateWarrantyRequest.NextMaintenanceDate.Value : warranty.NextMaintenanceDate;
            warranty.Priority = updateWarrantyRequest.Priority.HasValue ? updateWarrantyRequest.Priority.Value : warranty.Priority;

            _unitOfWork.GetRepository<Warranty>().UpdateAsync(warranty);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

    }
}
