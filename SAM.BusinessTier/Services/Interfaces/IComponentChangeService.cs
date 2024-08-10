using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IComponentChangeService
    {
        Task<bool> DeleteComponentChange(Guid warrantyDetailId);

    }
}
