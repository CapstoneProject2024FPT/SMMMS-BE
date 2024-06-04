﻿using AutoMapper;
using Azure.Core;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.User;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Paginate;
using SAM.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Implements
{
    public class CategoryService : BaseService<CategoryService>, ICategoryService
    {
        public CategoryService(IUnitOfWork<SamContext> unitOfWork, ILogger<CategoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewCategory(CreateNewCategoryRequest request)
        {
            _logger.LogInformation($"Start create new category: {request}");

            Category newCategory = _mapper.Map<Category>(request);

            if (request.MasterCategoryId != null)
            {
                var parentCategory = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                    predicate: x => x.Id.Equals(request.MasterCategoryId))
                    ?? throw new BadHttpRequestException(MessageConstant.Category.Parent_NotFoundFailedMessage);
                newCategory.Type = CategoryType.Child.GetDescriptionFromEnum();
            }
            else newCategory.Type = CategoryType.Parent.GetDescriptionFromEnum();


            await _unitOfWork.GetRepository<Category>().InsertAsync(newCategory);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Category.CreateCategoryFailedMessage);

            return newCategory.Id;
        }

        public async Task<IPaginate<GetCategoriesResponse>> GetCategories(CategoryFilter filter, PagingModel pagingModel)
        {
            IPaginate<GetCategoriesResponse> respone = await _unitOfWork.GetRepository<Category>().GetPagingListAsync(
               selector: x => _mapper.Map<GetCategoriesResponse>(x),
               filter: filter,
               page: pagingModel.page,
               size: pagingModel.size);
            return respone;
        }

        public async Task<GetCategoriesResponse> GetCategory(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Category.CategoryEmptyMessage);
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
            return _mapper.Map<GetCategoriesResponse>(category);
        }

        public async Task<bool> RemoveCategoryStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Category.CategoryEmptyMessage);
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
            category.Status = CategoryStatus.Inactive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }


        public async Task<bool> UpdateCategory(Guid categoryId, UpdateCategoryRequest request)
        {
            _logger.LogInformation($"Start updating product: {categoryId}");
            Category updateCategory = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(categoryId))
                ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);

            updateCategory.Name = string.IsNullOrEmpty(request.Name) ? updateCategory.Name : request.Name;
            updateCategory.Description = string.IsNullOrEmpty(request.Description) ? updateCategory.Description : request.Description;
            updateCategory.Status = request.Status.GetDescriptionFromEnum();
            updateCategory.MasterCategoryId = request.MasterCategoryId;

            if (request.MasterCategoryId != null)
            {
                updateCategory.Type = CategoryType.Child.GetDescriptionFromEnum();
            }
            else updateCategory.Type = CategoryType.Parent.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<Category>().UpdateAsync(updateCategory);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
