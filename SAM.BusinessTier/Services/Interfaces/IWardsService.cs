using SAM.BusinessTier.Payload.City;
using SAM.BusinessTier.Payload.Wards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IWardsService
    {
        Task<Guid> CreateNewWards(CreateNewWardRequest createNewWardsRequest);
        Task<bool> UpdateWards(Guid id, UpdateWardRequest updateWardsRequest);
        Task<ICollection<GetWardResponse>> GetWardsList(WardFilter filter);
        Task<GetWardResponse> GetWardsById(Guid id);
        Task<bool> RemoveWardsStatus(Guid id);
    }
}
