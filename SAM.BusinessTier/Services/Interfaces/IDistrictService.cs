using SAM.BusinessTier.Payload.City;
using SAM.BusinessTier.Payload.Discount;
using SAM.BusinessTier.Payload.Districts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IDistrictService
    {
        Task<Guid> CreateNewDistrict(CreateNewDistrictRequest createNewDistrictRequest);
        Task<bool> UpdateDistrict(Guid id, UpdateDistrictRequest updateDistrictsRequest);
        Task<ICollection<GetDistricResponse>> GetDistrictList(DistrictFilter filter);
        Task<GetDistricResponse> GetDistrictById(Guid id);
        Task<bool> RemoveDistrictStatus(Guid id);
    }
}
