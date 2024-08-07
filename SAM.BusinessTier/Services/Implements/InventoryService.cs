using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Enums.Other;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Payload.Inventory;
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
    public class InventoryService : BaseService<InventoryService>, IInventoryService
    {
        public InventoryService(IUnitOfWork<SamContext> unitOfWork, ILogger<InventoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<List<Guid>> CreateMultipleInventories(CreateNewInventoryRequest createNewInventoryRequest, int quantity)
        {
            var inventoryIds = new List<Guid>();

            if (createNewInventoryRequest.MachineryId.HasValue && createNewInventoryRequest.MachineComponentsId.HasValue)
            {
                throw new ArgumentException("Chỉ có thể tạo mã cho 1 máy cơ khí hoặc 1 bộ phận");
            }

            if (createNewInventoryRequest.MachineryId.HasValue)
            {
                var machinery = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                    predicate: x => x.Id.Equals(createNewInventoryRequest.MachineryId.Value),
                    include: x => x.Include(x => x.MachineryComponentParts)
                                    .ThenInclude(cp => cp.MachineComponents))
                    ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);
                if (machinery.MachineryComponentParts == null || !machinery.MachineryComponentParts.Any())
                {
                    throw new BadHttpRequestException("Có vẻ máy này chưa có bộ phận nào");
                }
                for (int i = 0; i < quantity; i++)
                {
                    var inventory = _mapper.Map<Inventory>(createNewInventoryRequest);
                    inventory.Id = Guid.NewGuid();
                    inventory.SerialNumber = TimeUtils.GetTimestamp(DateTime.Now) + i;
                    inventory.Status = InventoryStatus.Available.GetDescriptionFromEnum();
                    inventory.Type = InventoryType.Machinery.GetDescriptionFromEnum();
                    inventory.CreateDate = DateTime.Now;
                    inventory.Condition = InventoryCondition.New.GetDescriptionFromEnum();
                    inventory.IsRepaired = InventoryIsRepaired.New.GetDescriptionFromEnum();
                    await _unitOfWork.GetRepository<Inventory>().InsertAsync(inventory);
                    inventoryIds.Add(inventory.Id);

                    // Tạo Inventory cho các component của Machinery
                    foreach (var componentPart in machinery.MachineryComponentParts)
                    {
                        var componentInventory = new Inventory
                        {
                            Id = Guid.NewGuid(),
                            SerialNumber = TimeUtils.GetTimestamp(DateTime.Now) + i,
                            Status = InventoryStatus.Sold.GetDescriptionFromEnum(),
                            Type = InventoryType.Material.GetDescriptionFromEnum(),
                            CreateDate = DateTime.Now,
                            Condition = InventoryCondition.CurrentlyinUse.GetDescriptionFromEnum(),
                            IsRepaired = InventoryIsRepaired.New.GetDescriptionFromEnum(),
                            MasterInventoryId = inventory.Id,
                            MachineComponentsId = componentPart.MachineComponentsId
                        };
                        await _unitOfWork.GetRepository<Inventory>().InsertAsync(componentInventory);
                        inventoryIds.Add(componentInventory.Id);
                    }
                }
            }
            else if (createNewInventoryRequest.MachineComponentsId.HasValue)
            {
                var component = await _unitOfWork.GetRepository<MachineComponent>().SingleOrDefaultAsync(
                    predicate: x => x.Id == createNewInventoryRequest.MachineComponentsId.Value);
                if (component == null)
                {
                    throw new BadHttpRequestException(MessageConstant.MachineryComponents.MachineryComponentsNotFoundMessage);
                }

                for (int i = 0; i < quantity; i++)
                {
                    var inventory = _mapper.Map<Inventory>(createNewInventoryRequest);
                    inventory.Id = Guid.NewGuid();
                    inventory.SerialNumber = TimeUtils.GetTimestamp(DateTime.Now) + i;
                    inventory.Status = InventoryStatus.Available.GetDescriptionFromEnum();
                    inventory.Type = InventoryType.Material.GetDescriptionFromEnum();
                    inventory.CreateDate = TimeUtils.GetCurrentSEATime();
                    inventory.Condition = InventoryCondition.New.GetDescriptionFromEnum();
                    inventory.IsRepaired = InventoryIsRepaired.New.GetDescriptionFromEnum();
                    await _unitOfWork.GetRepository<Inventory>().InsertAsync(inventory);
                    inventoryIds.Add(inventory.Id);
                }
            }

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new BadHttpRequestException(MessageConstant.Inventory.CreateInventoryFailedMessage);

            return inventoryIds;
        }






        public async Task<GetInventoryResponse> GetInventoryById(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException(MessageConstant.Inventory.InventoryEmptyMessage);

            Inventory inventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(i => i.MachineComponents)
                               .Include(x => x.Machinery))
                ?? throw new BadHttpRequestException(MessageConstant.Inventory.NotFoundFailedMessage);

            var inventoryResponse = _mapper.Map<GetInventoryResponse>(inventory);
            inventoryResponse.ComponentName = inventory.MachineComponents?.Name;
            inventoryResponse.MachineryName = inventory.Machinery?.Name;
            return inventoryResponse;
        }

        public async Task<ICollection<GetInventoryResponse>> GetInventoryList(InventoryFilter filter)
        {
            var inventoryList = await _unitOfWork.GetRepository<Inventory>().GetListAsync(
                selector: x => x,
                orderBy: x => x.OrderByDescending(x => x.CreateDate),
                include: x => x.Include(i => i.MachineComponents)
                               .Include(i => i.Machinery),
                filter: filter);

            var inventoryResponseList = inventoryList.Select(inventory => {
                var inventoryResponse = _mapper.Map<GetInventoryResponse>(inventory);
                inventoryResponse.ComponentName = inventory.MachineComponents?.Name;
                inventoryResponse.MachineryName = inventory.Machinery?.Name;
                return inventoryResponse;
            }).ToList();

            return inventoryResponseList;
        }



        public async Task<bool> SwitchInventoryStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Brand.BrandEmptyMessage);
            Inventory inventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Inventory.NotFoundFailedMessage);
            inventory.Status = InventoryStatus.Sold.GetDescriptionFromEnum();
            inventory.SoldDate = DateTime.Now;
            _unitOfWork.GetRepository<Inventory>().UpdateAsync(inventory);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateInventory(Guid id, UpdateInventoryRequest updateInventoryRequest)
        {
            Inventory inventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Inventory.NotFoundFailedMessage);


            
            if (!updateInventoryRequest.Status.HasValue && !updateInventoryRequest.Type.HasValue)
            {
                throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
            }
            else
            {
                inventory.Status = updateInventoryRequest.Status.GetDescriptionFromEnum();
                inventory.Type = updateInventoryRequest.Type.GetDescriptionFromEnum();
            }

            _unitOfWork.GetRepository<Inventory>().UpdateAsync(inventory);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
