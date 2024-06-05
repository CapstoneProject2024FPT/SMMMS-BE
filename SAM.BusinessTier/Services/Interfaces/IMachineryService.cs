using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload;
using SAM.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAM.BusinessTier.Payload.Product;
using SAM.BusinessTier.Payload.Machinery;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IMachineryService
    {
        Task<Guid> CreateNewMachinerys(CreateNewMachineryRequest createNewProductRequest);
        Task<bool> UpdateMachinery(Guid id, UpdateMachineryRequest updateProductRequest);
        Task<IPaginate<GetMachinerysResponse>> GetMachineryList(MachineryFilter filter, PagingModel pagingModel);
        Task<ICollection<GetMachinerysResponse>> GetMachineryListNotIPaginate(MachineryFilter filter);
        Task<GetMachinerysResponse> GetMachineryById(Guid id);
        Task<bool> RemoveMachineryStatus(Guid id);
    }
}
