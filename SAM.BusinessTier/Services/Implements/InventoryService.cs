using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Brand;
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

            if (createNewInventoryRequest.MachineryId.HasValue)
            {
                var machinery = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                    predicate: x => x.Id == createNewInventoryRequest.MachineryId.Value);
                if (machinery == null)
                {
                    throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);
                }
            }

            for (int i = 0; i < quantity; i++)
            {
                var inventory = _mapper.Map<Inventory>(createNewInventoryRequest);
                inventory.Id = Guid.NewGuid();
                inventory.SerialNumber = TimeUtils.GetTimestamp(TimeUtils.GetCurrentSEATime());
                inventory.Status = InventoryStatus.Available.GetDescriptionFromEnum();
                inventory.Type = InventoryType.Material.GetDescriptionFromEnum();
                inventory.CreateDate = DateTime.Now;

                await _unitOfWork.GetRepository<Inventory>().InsertAsync(inventory);
                inventoryIds.Add(inventory.Id);
            }

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new BadHttpRequestException(MessageConstant.Inventory.CreateInventoryFailedMessage);

            return inventoryIds;
        }


        public async Task<GetInventoryResponse> GetInventoryById(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Inventory.InventoryEmptyMessage);
            Inventory inventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Inventory.NotFoundFailedMessage);
            return _mapper.Map<GetInventoryResponse>(inventory);
        }

        public async Task<ICollection<GetInventoryResponse>> GetInventoryList(InventoryFilter filter)
        {
            ICollection<GetInventoryResponse> respone = await _unitOfWork.GetRepository<Inventory>().GetListAsync(
               selector: x => _mapper.Map<GetInventoryResponse>(x),
               filter: filter);
            return respone;
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


            inventory.Status = updateInventoryRequest.Status.GetDescriptionFromEnum();
            inventory.Type = updateInventoryRequest.Type.GetDescriptionFromEnum();


            _unitOfWork.GetRepository<Inventory>().UpdateAsync(inventory);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
