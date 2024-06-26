using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.News;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Implements
{
    public class NewsService : BaseService<CategoryService>, INewsService
    {
        public NewsService(IUnitOfWork<SamContext> unitOfWork, ILogger<CategoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewNews(CreateNewsRequest createNewsRequest)
        {
            throw new NotImplementedException();
        }

        public Task<GetBrandResponse> GetNewsById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetNewsReponse>> GetNewsList(NewsFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveNewsStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNews(Guid id, UpdateNewsRequest updateNewsRequest)
        {
            throw new NotImplementedException();
        }
    }
}
