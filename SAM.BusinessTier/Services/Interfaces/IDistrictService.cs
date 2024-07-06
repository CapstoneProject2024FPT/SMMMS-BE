
using SAM.BusinessTier.Payload.Districts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IDistrictService
    {
        Task<Guid> CreateNewDistrict(CreateNewDistrictRequest createNewDistrictRequest);
        Task<bool> UpdateDistrict(Guid id, UpdateDistrictRequest updateDistrictRequest);
        Task<ICollection<GetDistrictResponse>> GetDistrictList(DistrictFilter filter);
        Task<GetDistrictResponse> GetDistrictById(Guid id);
        Task<bool> RemoveDistrictStatus(Guid id);
    }
}
