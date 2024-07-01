using SAM.BusinessTier.Payload.NewsCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface INewsCategoryService
    {
        Task<Guid> CreateNewNewsCategory(CreateNewNewsCategoryRequest createNewNewsCategoryRequest);
        Task<bool> UpdateNewsCategory(Guid id, UpdateNewsCategoryRequest updateCategoryRequest);
        Task<ICollection<GetNewsCategoriesResponse>> GetNewsCategories(NewsCategoryFilter filter);
        Task<GetNewsCategoriesResponse> GetNewsCategory(Guid id);
        Task<bool> RemoveNewsCategoryStatus(Guid id);
    }
}
