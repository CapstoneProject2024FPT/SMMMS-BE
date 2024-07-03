using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.News;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            if (request.NewsCategoryId.HasValue)
            {
                var newsCategory = await _unitOfWork.GetRepository<NewsCategory>().SingleOrDefaultAsync(
                    predicate: m => m.Id == request.NewsCategoryId.Value)
                    ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);

            }

            News newNews = new()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                NewsContent = request.NewsContent,
                Cover = request.Cover,
                Status = NewsStatus.Active.GetDescriptionFromEnum(),
                Type = NewsTypes.Normal.GetDescriptionFromEnum(),
                NewsCategoryId = request.NewsCategoryId,
                AccountId = request.AccountId,
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



        public async Task<GetNewsResponse> GetNewsById(Guid id)
{
            var news = await _unitOfWork.GetRepository<News>()
                .SingleOrDefaultAsync(
                    predicate: x => x.Id == id,
                    include: x => x.Include(x => x.NewsCategory)
                                   .Include(x => x.Account)
                                   .Include(x => x.NewsImages))
                ?? throw new BadHttpRequestException(MessageConstant.News.NewsNotFoundMessage);

            var newsResponse = new GetNewsResponse
            {
                Id = news.Id,
                Title = news.Title,
                Description = news.Description,
                NewsContent = news.NewsContent,
                Cover = news.Cover,
                Status = EnumUtil.ParseEnum<NewsStatus>(news.Status),
                Type = EnumUtil.ParseEnum<NewsTypes>(news.Type),
                CreateDate = news.CreateDate,
                NewsCategory = new NewsCategoryResponse
                {
                    NewsCategoryId = news.NewsCategory.Id,
                    Name = news.NewsCategory.Name,
                    Description = news.NewsCategory.Description
                },
                Account = new AccountResponse
                {
                    Id = news.Account.Id,
                    FullName = news.Account.FullName,
                    Role = EnumUtil.ParseEnum<RoleEnum>(news.Account.Role)
                },
                ImgList = news.NewsImages.Select(image => new NewsImageResponse
                {
                    Id = image.Id,
                    ImgUrl = image.ImgUrl,
                    CreateDate = image.CreateDate,

                }).ToList()
            };

            return newsResponse;
        }


        public async Task<ICollection<GetNewsResponse>> GetNewsList(NewsFilter filter)
        {
            var newsList = await _unitOfWork.GetRepository<News>()
                .GetListAsync(
                    selector: x => x,
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.CreateDate),
                    include: x => x.Include(x => x.NewsCategory)
                                   .Include(x => x.Account)
                                   .Include(x => x.NewsImages))
                ?? throw new BadHttpRequestException(MessageConstant.News.NewsNotFoundMessage);

            var newsResponses = newsList.Select(news => new GetNewsResponse
            {
                Id = news.Id,
                Title = news.Title,
                Description = news.Description,
                NewsContent = news.NewsContent,
                Cover = news.Cover,
                Status = EnumUtil.ParseEnum<NewsStatus>(news.Status),
                Type = EnumUtil.ParseEnum<NewsTypes>(news.Type),
                CreateDate = news.CreateDate,
                NewsCategory = new NewsCategoryResponse
                {
                    NewsCategoryId = news.NewsCategory.Id,
                    Name = news.NewsCategory.Name,
                    Description = news.NewsCategory.Description
                },
                Account = new AccountResponse
                {
                    Id = news.Account.Id,
                    FullName = news.Account.FullName,
                    Role = EnumUtil.ParseEnum<RoleEnum>(news.Account.Role)
                },
                ImgList = news.NewsImages.Select(image => new NewsImageResponse
                {
                    Id = image.Id,
                    ImgUrl = image.ImgUrl,
                    CreateDate = image.CreateDate,

                }).ToList(),

            }).ToList();

            return newsResponses;
        }


        public async Task<bool> RemoveNewsStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.News.EmptyNewsIdMessage);
            News news = await _unitOfWork.GetRepository<News>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.News.NewsNotFoundMessage);
            news.Status = NewsStatus.Inactive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<News>().UpdateAsync(news);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }


        public async Task<bool> UpdateNews(Guid id, UpdateNewsRequest updateNewsRequest)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.News.EmptyNewsIdMessage);
            News news = await _unitOfWork.GetRepository<News>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.News.NewsNameExisted);

            NewsCategory newsCategory = await _unitOfWork.GetRepository<NewsCategory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateNewsRequest.NewsCategoryId))
            ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
            news.NewsCategory = newsCategory;

            news.Title = string.IsNullOrEmpty(updateNewsRequest.Title) ? news.Title : updateNewsRequest.Title;
            news.Description = string.IsNullOrEmpty(updateNewsRequest.Description) ? news.Description : updateNewsRequest.Description;
            news.NewsContent = string.IsNullOrEmpty(updateNewsRequest.NewsContent) ? news.NewsContent : updateNewsRequest.NewsContent;
            news.Cover = string.IsNullOrEmpty(updateNewsRequest.Cover) ? news.Cover : updateNewsRequest.Cover;
            news.Status = updateNewsRequest.Status.GetDescriptionFromEnum();
            news.Type = updateNewsRequest.Type.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<News>().UpdateAsync(news);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }
    }
}
