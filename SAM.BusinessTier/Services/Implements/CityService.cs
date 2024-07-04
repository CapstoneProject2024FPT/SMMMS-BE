using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.City;
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
    public class CityService : BaseService<CityService>, ICityService
    {
        public CityService(IUnitOfWork<SamContext> unitOfWork, ILogger<CityService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewCity(CreateNewCityRequest createNewCityRequest)
        {
            throw new NotImplementedException();
        }

        public Task<GetCityResponse> GetCityById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetCityResponse>> GetCityList(CityFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveCityStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCity(Guid id, UpdateCityRequest updateCityRequest)
        {
            throw new NotImplementedException();
        }
    }
}
