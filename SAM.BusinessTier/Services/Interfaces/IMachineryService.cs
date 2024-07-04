using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload;
using SAM.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.Order;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IMachineryService
    {
        Task<Guid> CreateNewMachinerys(CreateNewMachineryRequest createNewProductRequest);
        Task<bool> UpdateMachinery(Guid id, UpdateMachineryRequest updateProductRequest);
        Task<bool> UpdateStatusMachineryResponse(Guid id, UpdateStatusMachineryResponse updateStatusMachineryResponse);
        Task<ICollection<GetMachinerySpecificationsRespone>> GetMachineryListNoPagingNate(MachineryFilter filter);
        Task<IPaginate<GetMachinerySpecificationsRespone>> GetMachineryList(MachineryFilter filter, PagingModel pagingModel);
        Task<bool> RemoveMachineryStatus(Guid id);
        Task<GetMachinerySpecificationsRespone> GetMachinerySpecificationsDetail(Guid id);
    }
}
