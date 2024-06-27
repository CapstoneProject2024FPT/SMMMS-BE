using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Inventory;
using SAM.BusinessTier.Payload.Origin;
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
    public class InventoryService : BaseService<InventoryService>, IInventoryService
    {
        public InventoryService(IUnitOfWork<SamContext> unitOfWork, ILogger<InventoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewInventory(CreateNewInventoryRequest createNewInventoryRequest)
        {
            if (createNewInventoryRequest.MachineryId.HasValue)
            {
                var machinery = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                    predicate: c => c.Id == createNewInventoryRequest.MachineryId.Value);
                if (machinery == null)
                {
                    throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);
                }
            }
            Inventory inventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync();
            if (inventory != null) throw new BadHttpRequestException(MessageConstant.Inventory.InventoryEmptyMessage);
            inventory = _mapper.Map<Inventory>(createNewInventoryRequest);
            await _unitOfWork.GetRepository<Inventory>().InsertAsync(inventory);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new BadHttpRequestException(MessageConstant.Inventory.CreateInventoryFailedMessage);
            return inventory.Id;
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

        public Task<bool> RemoveInventoryStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateInventory(Guid id, UpdateInventoryRequest updateInventoryRequest)
        {
            throw new NotImplementedException();
        }
    }
}
