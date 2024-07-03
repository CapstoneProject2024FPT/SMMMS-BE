using Microsoft.AspNetCore.Mvc.RazorPages;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.News;
using SAM.DataTier.Paginate;
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
        Task<IPaginate<GetNewsResponse>> GetNewsList(NewsFilter filter, PagingModel pagingModel);
        Task<ICollection<GetNewsResponse>> GetNewsListNoPagingNate(NewsFilter filter);
        Task<GetNewsResponse> GetNewsById(Guid id);
        Task<bool> RemoveNewsStatus(Guid id);
    }
}
