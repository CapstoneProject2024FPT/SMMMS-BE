using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IBrandService
    {
        Task<Guid> CreateNewBrand(CreateNewBrandRequest createNewBrandRequest);
        Task<bool> UpdateBrand(Guid id, UpdateBrandRequest updateBrandRequest);
        Task<ICollection<GetBrandResponse>> GetBrandList(BrandFilter filter);
        Task<GetBrandResponse> GetBrandById(Guid id);
        Task<bool> RemoveBrandStatus(Guid id);
    }
}
