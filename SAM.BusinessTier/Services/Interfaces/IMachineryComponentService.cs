using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Inventory;
using SAM.BusinessTier.Payload.MachineryComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IMachineryComponentService
    {
        Task<Guid> CreateNewMachineryComponents(CreateNewMachineryComponentRequest createMachineryComponentRequest);
        Task<bool> UpdateMachineryComponent(Guid id, UpdateMachineryComponentRequest updateMachineryComponentRequest);
        Task<ICollection<GetMachineryComponentResponse>> GetMachineryComponentList(MachineryComponentFilter filter);
        Task<GetMachineryComponentResponse> GetMachineryComponentById(Guid id);
        Task<bool> RemoveMachineryComponentStatus(Guid id);
    }
}
