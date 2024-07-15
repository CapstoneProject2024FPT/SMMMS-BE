using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Inventory;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.MachineryComponent;
using SAM.DataTier.Paginate;
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
        Task<ICollection<GetMachineryComponentResponse>> GetMachineryComponentListNoPagingNate(MachineryComponentFilter filter);
        Task<IPaginate<GetMachineryComponentResponse>> GetMachineryComponentList(MachineryComponentFilter filter, PagingModel pagingModel);
        Task<GetMachineryComponentResponse> GetMachineryComponentById(Guid id);
        Task<bool> RemoveMachineryComponentStatus(Guid id);
    }
}
