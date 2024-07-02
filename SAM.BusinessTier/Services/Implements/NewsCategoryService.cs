using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.NewsCategory;
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
    public class NewsCategoryService : BaseService<NewsCategoryService>, INewsCategoryService
    {
        public NewsCategoryService(IUnitOfWork<SamContext> unitOfWork, ILogger<NewsCategoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewNewsCategory(CreateNewNewsCategoryRequest createNewNewsCategoryRequest)
        {
            NewsCategory newCategory = await _unitOfWork.GetRepository<NewsCategory>().SingleOrDefaultAsync(
                predicate: x => x.Name.Equals(createNewNewsCategoryRequest.Name));
            if (newCategory != null) throw new BadHttpRequestException(MessageConstant.Category.CategoryExistedMessage);
            newCategory = _mapper.Map<NewsCategory>(createNewNewsCategoryRequest);
            newCategory.Id = Guid.NewGuid();
            newCategory.Status = NewsCategoryStatus.Active.GetDescriptionFromEnum();
            newCategory.CreateDate = DateTime.Now;

            await _unitOfWork.GetRepository<NewsCategory>().InsertAsync(newCategory);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Category.CreateCategoryFailedMessage);

            return newCategory.Id;
        }

        public async Task<ICollection<GetNewsCategoriesResponse>> GetNewsCategories(NewsCategoryFilter filter)
        {
            ICollection<GetNewsCategoriesResponse> respone = await _unitOfWork.GetRepository<NewsCategory>().GetListAsync(
               selector: x => _mapper.Map<GetNewsCategoriesResponse>(x),
               filter: filter);
            return respone;
        }

        public async Task<GetNewsCategoriesResponse> GetNewsCategory(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Category.CategoryEmptyMessage);
            NewsCategory category = await _unitOfWork.GetRepository<NewsCategory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
            return _mapper.Map<GetNewsCategoriesResponse>(category);
        }

        public async Task<bool> RemoveNewsCategoryStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Category.CategoryEmptyMessage);
            NewsCategory category = await _unitOfWork.GetRepository<NewsCategory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
            category.Status = CategoryStatus.Inactive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<NewsCategory>().UpdateAsync(category);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateNewsCategory(Guid id, UpdateNewsCategoryRequest updateCategoryRequest)
        {
            NewsCategory updateCategory = await _unitOfWork.GetRepository<NewsCategory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);

            updateCategory.Name = string.IsNullOrEmpty(updateCategoryRequest.Name) ? updateCategory.Name : updateCategoryRequest.Name;
            updateCategory.Description = string.IsNullOrEmpty(updateCategoryRequest.Description) ? updateCategory.Description : updateCategoryRequest.Description;
            updateCategory.Status = updateCategoryRequest.Status.GetDescriptionFromEnum();




            _unitOfWork.GetRepository<NewsCategory>().UpdateAsync(updateCategory);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
