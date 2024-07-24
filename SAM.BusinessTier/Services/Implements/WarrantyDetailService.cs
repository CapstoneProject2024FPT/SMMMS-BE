using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Enums.Other;
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
            // Lấy danh sách WarrantyDetail theo bộ lọc
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
                        Staff = detail.Account != null ? new OrderUserResponse
                        {
                            Id = detail.Account.Id,
                            FullName = detail.Account.FullName,
                            Role = EnumUtil.ParseEnum<RoleEnum>(detail.Account.Role)
                        } : null,
                        InventoryChanges = new List<InventoryChangeResponse>() // Khởi tạo danh sách trống
                    },
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.StartDate),
                    include: x => x.Include(x => x.Account)
                ) ?? throw new BadHttpRequestException(MessageConstant.WarrantyDetail.WarrantyDetailNotFoundMessage);

            // Lấy danh sách ID của WarrantyDetail
            var warrantyDetailIds = warrantyDetails.Select(wd => wd.Id).ToList();

            // Truy vấn thông tin InventoryChange liên quan đến các WarrantyDetail
            var inventoryChanges = await _unitOfWork.GetRepository<InventoryChange>()
                .GetListAsync(
                    selector: change => new InventoryChangeResponse
                    {
                        OldInventory = new InventoryInWarrantyDetailResponse
                        {
                            Id = change.OldInventoryId,
                        },
                        NewInventory = new InventoryInWarrantyDetailResponse
                        {
                            Id = change.NewInventoryId,
                        }
                    },
                    predicate: change => warrantyDetailIds.Contains(change.WarrantyDetailId)
                );

            // Truy vấn chi tiết về các Inventory từ bảng Inventory
            var inventoryIds = inventoryChanges.SelectMany(ic => new[] { ic.OldInventory.Id, ic.NewInventory.Id }).Distinct().ToList();
            var inventories = await _unitOfWork.GetRepository<Inventory>()
                .GetListAsync(
                    selector: inventory => new InventoryInWarrantyDetailResponse
                    {
                        Id = inventory.Id,
                        SerialNumber = inventory.SerialNumber,
                        Type = EnumUtil.ParseEnum<InventoryType>(inventory.Type)
                    },
                    predicate: inventory => inventoryIds.Contains(inventory.Id)
                );

            // Cập nhật thông tin chi tiết cho InventoryChangeResponse
            foreach (var change in inventoryChanges)
            {
                change.OldInventory = inventories.SingleOrDefault(inv => inv.Id == change.OldInventory.Id);
                change.NewInventory = inventories.SingleOrDefault(inv => inv.Id == change.NewInventory.Id);
            }

            // Gán InventoryChanges cho từng WarrantyDetail
            foreach (var warrantyDetail in warrantyDetails)
            {
                warrantyDetail.InventoryChanges = inventoryChanges
                    .Where(ic => ic.OldInventory.Id == warrantyDetail.Id || ic.NewInventory.Id == warrantyDetail.Id)
                    .ToList();
            }

            return warrantyDetails;
        }



        public async Task<GetWarrantyDetailResponse> GetWarrantyDetailById(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException("Invalid warranty detail ID.");

            // Lấy thông tin WarrantyDetail
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
                        Staff = detail.Account != null ? new OrderUserResponse
                        {
                            Id = detail.Account.Id,
                            FullName = detail.Account.FullName,
                            Role = EnumUtil.ParseEnum<RoleEnum>(detail.Account.Role)
                        } : null,
                        InventoryChanges = new List<InventoryChangeResponse>()
                    },
                    predicate: detail => detail.Id == id,
                    include: x => x.Include(x => x.Account)
                                  .Include(x => x.Warranty.Inventory) // Include Inventory for detailed information
                                  .ThenInclude(i => i.MachineComponents) // Include MachineComponents
                );

            if (warrantyDetail == null)
            {
                throw new BadHttpRequestException($"Warranty detail with ID {id} not found.");
            }

            // Truy vấn thông tin InventoryChange liên quan
            var inventoryChanges = await _unitOfWork.GetRepository<InventoryChange>()
                .GetListAsync(
                    selector: change => new InventoryChangeResponse
                    {
                        OldInventory = new InventoryInWarrantyDetailResponse
                        {
                            Id = change.OldInventoryId,
                        },
                        NewInventory = new InventoryInWarrantyDetailResponse
                        {
                            Id = change.NewInventoryId,
                        }
                    },
                    predicate: change => change.WarrantyDetailId == id
                );

            // Truy vấn chi tiết về các Inventory từ bảng Inventory
            var inventoryIds = inventoryChanges.SelectMany(ic => new[] { ic.OldInventory.Id, ic.NewInventory.Id }).Distinct().ToList();
            var inventories = await _unitOfWork.GetRepository<Inventory>()
                .GetListAsync(
                    selector: inventory => new InventoryInWarrantyDetailResponse
                    {
                        Id = inventory.Id,
                        SerialNumber = inventory.SerialNumber,
                        Type = EnumUtil.ParseEnum<InventoryType>(inventory.Type),
                        ComponentName = inventory.MachineComponents != null ? inventory.MachineComponents.Name : null
                    },
                    predicate: inventory => inventoryIds.Contains(inventory.Id)
                );

            // Cập nhật thông tin chi tiết cho InventoryChangeResponse
            foreach (var change in inventoryChanges)
            {
                change.OldInventory = inventories.SingleOrDefault(inv => inv.Id == change.OldInventory.Id);
                change.NewInventory = inventories.SingleOrDefault(inv => inv.Id == change.NewInventory.Id);
            }

            // Cập nhật danh sách InventoryChanges vào response
            warrantyDetail.InventoryChanges = inventoryChanges.ToList();

            return warrantyDetail;
        }



        public Task<bool> RemoveWarrantyDetailStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWarrantyDetail(Guid id, UpdateWarrantyDetailRequest updateDetailRequest)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException(MessageConstant.WarrantyDetail.EmptyWarrantyDetailIdMessage);

            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            // Truy xuất WarrantyDetail
            WarrantyDetail warrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.WarrantyDetail.WarrantyDetailNotFoundMessage);

            // Cập nhật thông tin WarrantyDetail
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

            if (updateDetailRequest.InventoryUpdates != null && updateDetailRequest.InventoryUpdates.Any())
            {
                foreach (var inventoryUpdate in updateDetailRequest.InventoryUpdates)
                {
                    if (inventoryUpdate.NewInventoryId.HasValue && inventoryUpdate.OldInventoryId.HasValue)
                    {
                        var oldInventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                            predicate: x => x.Id.Equals(inventoryUpdate.OldInventoryId.Value))
                        ?? throw new BadHttpRequestException(MessageConstant.Inventory.NotFoundFailedMessage);

                        var newInventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                            predicate: x => x.Id.Equals(inventoryUpdate.NewInventoryId.Value))
                        ?? throw new BadHttpRequestException(MessageConstant.Inventory.NotFoundFailedMessage);

                        newInventory.MasterInventoryId = oldInventory.MasterInventoryId; 
                        newInventory.Condition = InventoryCondition.CurrentlyinUse.GetDescriptionFromEnum();
                        newInventory.Status = InventoryStatus.Sold.GetDescriptionFromEnum();
                        _unitOfWork.GetRepository<Inventory>().UpdateAsync(newInventory);

                        oldInventory.Condition = InventoryCondition.Old.GetDescriptionFromEnum();
                        oldInventory.IsRepaired = InventoryIsRepaired.IsRepaired.GetDescriptionFromEnum();
                        _unitOfWork.GetRepository<Inventory>().UpdateAsync(oldInventory);

                        var inventoryChange = new InventoryChange
                        {
                            Id = new Guid(),
                            WarrantyDetailId = warrantyDetail.Id,
                            NewInventoryId = inventoryUpdate.NewInventoryId.Value,
                            OldInventoryId = inventoryUpdate.OldInventoryId.Value
                        };
                        await _unitOfWork.GetRepository<InventoryChange>().InsertAsync(inventoryChange);
                    }
                }
            }

            // Xử lý trạng thái hoàn thành
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

                var nextWarrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
                    predicate: wd => wd.WarrantyId == warrantyDetail.WarrantyId && wd.StartDate > warrantyDetail.StartDate && wd.Status != WarrantyDetailStatus.Completed.GetDescriptionFromEnum(),
                    orderBy: q => q.OrderBy(wd => wd.StartDate));

                Warranty warranty = await _unitOfWork.GetRepository<Warranty>().SingleOrDefaultAsync(
                    predicate: w => w.Id == warrantyDetail.WarrantyId)
                ?? throw new BadHttpRequestException(MessageConstant.Warranty.WarrantyNotFoundMessage);

                if (nextWarrantyDetail != null)
                {
                    warranty.NextMaintenanceDate = nextWarrantyDetail.StartDate;
                }
                else
                {
                    warranty.NextMaintenanceDate = null;
                }

                _unitOfWork.GetRepository<Warranty>().UpdateAsync(warranty);
            }

            _unitOfWork.GetRepository<WarrantyDetail>().UpdateAsync(warrantyDetail);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }


    }
}
