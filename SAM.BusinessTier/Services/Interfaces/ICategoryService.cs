﻿using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.User;
using SAM.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Implements
{
    public interface ICategoryService
    {
        Task<Guid> CreateNewCategory(CreateNewCategoryRequest createNewCategoryRequest);
        Task<bool> UpdateCategory(Guid id, UpdateCategoryRequest updateCategoryRequest);
        Task<IPaginate<GetCategoriesResponse>> GetCategories(CategoryFilter filter, PagingModel pagingModel);
        Task<GetCategoriesResponse> GetCategory(Guid id);
        Task<bool> RemoveCategoryStatus(Guid id);
    }
}
