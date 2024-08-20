using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Discount;
using SAM.BusinessTier.Payload.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<Guid> CreateNewDiscounts(CreateNewDiscountRequest createNewDiscountRequest);
        Task<bool> UpdateDiscount(Guid id, UpdateDiscountRequest updateDiscountRequest);
        Task<ICollection<GetDiscountResponse>> GetDiscountList(DiscountFilter filter);
        Task<GetDiscountResponse> GetDiscountById(Guid id);
        Task<bool> RemoveDiscountStatus(Guid id);
        Task<bool> AddDiscountToCategories(Guid id, List<Guid> request);
    }
}
