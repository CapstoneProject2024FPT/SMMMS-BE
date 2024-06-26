using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<Guid> CreateNewInventory(CreateNewInventoryRequest createNewInventoryRequest);
        Task<bool> UpdateInventory(Guid id, UpdateInventoryRequest updateInventoryRequest);
        Task<ICollection<GetInventoryResponse>> GetInventoryList(InventoryFilter filter);
        Task<GetBrandResponse> GetInventoryById(Guid id);
        Task<bool> RemoveInventoryStatus(Guid id);
    }
}
