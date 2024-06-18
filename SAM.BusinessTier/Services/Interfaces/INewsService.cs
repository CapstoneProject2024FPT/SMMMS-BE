using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface INewsService
    {
        Task<Guid> CreateNewsRequest(CreateNewsRequest createNewsRequest);
    }
}
