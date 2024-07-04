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
    public interface IDistrictsService
    {
        Task<Guid> CreateNewDistricts(CreateNewDistrictRequest createNewDistrictRequest);
        Task<bool> UpdateDistrict(Guid id, UpdateDistrictRequest updateDistrictsRequest);
        Task<ICollection<GetDistricResponse>> GetDistrictsList(DistrictFilter filter);
        Task<GetDistricResponse> GetDistrictsById(Guid id);
        Task<bool> RemoveDistrictsStatus(Guid id);
    }
}
