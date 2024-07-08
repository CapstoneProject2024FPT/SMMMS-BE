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
        Task<List<Guid>> CreateMultipleInventories(CreateNewInventoryRequest createNewInventoryRequest, int quantity);
        Task<bool> UpdateInventory(Guid id, UpdateInventoryRequest updateInventoryRequest);
        Task<ICollection<GetInventoryResponse>> GetInventoryList(InventoryFilter filter);
        Task<GetInventoryResponse> GetInventoryById(Guid id);
        Task<bool> SwitchInventoryStatus(Guid id);
    }
}
