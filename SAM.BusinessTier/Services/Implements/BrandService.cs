using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Brand;
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
    public class BrandService : BaseService<BrandService>, IBrandService
    {
        public BrandService(IUnitOfWork<SamContext> unitOfWork, ILogger<BrandService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewBrand(CreateNewBrandRequest createNewBrandRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetBrandResponse>> GetBrands(BrandFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<GetBrandResponse> GetCategory(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveCategoryStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBrand(Guid id, UpdateBrandRequest updateBrandRequest)
        {
            throw new NotImplementedException();
        }
    }
}
