using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IInventoryChangeService
    {
        Task<bool> DeleteInventoryChange(Guid newInventoryId);

    }
}
