using SAM.BusinessTier.Payload.City;
using SAM.BusinessTier.Payload.Wards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IWardService
    {
        Task<Guid> CreateNewWard(CreateNewWardRequest createNewWardsRequest);
        Task<bool> UpdateWard(Guid id, UpdateWardRequest updateWardsRequest);
        Task<ICollection<GetWardResponse>> GetWardList(WardFilter filter);
        Task<GetWardResponse> GetWardById(Guid id);
        Task<bool> RemoveWardStatus(Guid id);
    }
}
