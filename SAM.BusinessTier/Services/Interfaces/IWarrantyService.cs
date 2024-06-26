using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Warranty;
using SAM.BusinessTier.Payload.WarrantyDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IWarrantyService
    {
        Task<Guid> CreateNewWarranty(CreateNewWarrantyRequest createNewWarrantyRequest);
        Task<bool> UpdateWarranty(Guid id, UpdateWarrantyRequest updateWarrantyRequest);
        Task<ICollection<GetWarrantyInforResponse>> GetRankList(WarrantyFilter filter);
        Task<GetBrandResponse> GetWarrantyById(Guid id);
        Task<bool> RemoveWarrantyStatus(Guid id);
    }
}
