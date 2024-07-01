using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.NewsCategory;
using SAM.BusinessTier.Services.Interfaces;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Implements
{
    public class NewsCategoryService : BaseService<NewsCategoryService>, INewsCategoryService
    {
        public NewsCategoryService(IUnitOfWork<SamContext> unitOfWork, ILogger<NewsCategoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewNewsCategory(CreateNewNewsCategoryRequest createNewNewsCategoryRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetNewsCategoriesResponse>> GetNewsCategories(NewsCategoryFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<GetNewsCategoriesResponse> GetNewsCategory(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveNewsCategoryStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNewsCategory(Guid id, UpdateNewsCategoryRequest updateCategoryRequest)
        {
            throw new NotImplementedException();
        }
    }
}
