using SAM.BusinessTier.Payload;
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
        Task<bool> AddDiscountToAccount(Guid id, List<Guid> request);
        Task<Guid> CreateNewCategory(CreateNewCategoryRequest createNewCategoryRequest);
        Task<bool> UpdateCategory(Guid id, UpdateCategoryRequest updateCategoryRequest);
        Task<ICollection<GetCategoriesResponse>> GetCategories(CategoryFilter filter);
        Task<GetCategoriesResponse> GetCategory(Guid id);
        Task<bool> RemoveCategoryStatus(Guid id);
    }
}
