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
        Task<ICollection<GetOriginResponse>> GetOriginList(OriginFilter filter);
        Task<GetOriginResponse> GetOriginById(Guid id);
        Task<bool> RemoveOriginStatus(Guid id);
    }
}
