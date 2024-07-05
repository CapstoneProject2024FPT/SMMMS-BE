using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.City;
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
    public class CityService : BaseService<CityService>, ICityService
    {
        public CityService(IUnitOfWork<SamContext> unitOfWork, ILogger<CityService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewCity(CreateNewCityRequest createNewCityRequest)
        {
            City city = await _unitOfWork.GetRepository<City>().SingleOrDefaultAsync(
                predicate: x => x.Name.Equals(createNewCityRequest.Name));
            if (city != null) throw new BadHttpRequestException(MessageConstant.City.CityExistedMessage);
            city = _mapper.Map<City>(createNewCityRequest);
            city.Id = Guid.NewGuid();
            city.Status = CityStatus.Active.GetDescriptionFromEnum();

            await _unitOfWork.GetRepository<City>().InsertAsync(city);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.City.CreateCityFailedMessage);

            return city.Id;
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
