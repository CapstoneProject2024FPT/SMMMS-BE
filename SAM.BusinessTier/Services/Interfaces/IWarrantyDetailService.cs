using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Rank;
using SAM.BusinessTier.Payload.Warranty;
using SAM.BusinessTier.Payload.WarrantyDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IWarrantyDetailService
    {
        Task<bool> UpdateWarrantyDetail(Guid id, UpdateWarrantyDetailRequest updateWarrantyDetailRequest);
        Task<ICollection<GetWarrantyDetailResponse>> GetWarrantyDetailList(WarrantyDetailFilter filter);
        Task<GetWarrantyDetailResponse> GetWarrantyDetailById(Guid id);
        Task<Guid> CreateOrderForReplacedComponents(CreateNewOrderForWarrantyComponentRequest createNewOrderForWarrantyComponent);
    }
}
