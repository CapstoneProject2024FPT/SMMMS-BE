using SAM.BusinessTier.Payload.Brand;
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
        Task<Guid> CreateNewNews(CreateNewsRequest createNewsRequest);
        Task<bool> UpdateNews(Guid id, UpdateNewsRequest updateNewsRequest);
        Task<ICollection<GetNewsReponse>> GetNewsList(NewsFilter filter);
        Task<GetNewsReponse> GetNewsById(Guid id);
        Task<bool> RemoveNewsStatus(Guid id);
    }
}
