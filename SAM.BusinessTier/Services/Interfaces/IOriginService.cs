using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Origin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IOriginService
    {
        Task<Guid> CreateNewOrigin(CreateNewOriginRequest createNewOriginRequest);
        Task<bool> UpdateOrigin(Guid id, UpdateOriginRequest updateOriginRequest);
        Task<ICollection<GetOriginResponse>> GetOrigins(OriginFilter filter);
        Task<GetOriginResponse> GetOrigin(Guid id);
        Task<bool> RemoveOriginStatus(Guid id);
    }
}
