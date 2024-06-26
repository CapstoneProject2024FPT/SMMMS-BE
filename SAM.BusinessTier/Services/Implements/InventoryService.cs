using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Inventory;
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

        public Task<Guid> CreateNewInventory(CreateNewInventoryRequest createNewInventoryRequest)
        {
            throw new NotImplementedException();
        }

        public Task<GetBrandResponse> GetInventoryById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetInventoryResponse>> GetInventoryList(InventoryFilter filter)
        {
            throw new NotImplementedException();
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
