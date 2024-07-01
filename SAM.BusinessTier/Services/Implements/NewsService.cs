using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
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

        public async Task<Guid> CreateNewNews(CreateNewsRequest request)
        {
            var currentUser = GetUsernameFromJwt();
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser));
            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            if (request.MachineryId.HasValue)
            {
                var machinery = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                    predicate: m => m.Id == request.MachineryId.Value)
                ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);
            }

            News newNews = new()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                NewsContent = request.NewsContent,
                Cover = request.Cover,
                Status = NewsStatus.Active.GetDescriptionFromEnum(),
                MachineryId = request.MachineryId,
                AccountId = account.Id,
                CreateDate = DateTime.Now,
            };

            var imagesUrl = new List<NewsImage>();
            foreach (var img in request.Image)
            {
                imagesUrl.Add(new NewsImage
                {
                    Id = Guid.NewGuid(),
                    ImgUrl = img.ImageURL,
                    CreateDate = DateTime.Now,
                    NewsId = newNews.Id,
                });
            }

            await _unitOfWork.GetRepository<News>().InsertAsync(newNews);
            await _unitOfWork.GetRepository<NewsImage>().InsertRangeAsync(imagesUrl);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.News.CreateNewNewsFailedMessage);

            return newNews.Id;
        }



        public Task<GetNewsReponse> GetNewsById(Guid id)
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
