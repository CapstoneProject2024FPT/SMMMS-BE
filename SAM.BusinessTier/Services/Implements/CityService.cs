using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.City;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Payload.News;
using SAM.BusinessTier.Payload.NewsCategory;
using SAM.BusinessTier.Payload.Wards;
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
        public CityService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<CityService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
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

        public async Task<GetCityResponse> GetCityById(Guid id)
        {
            var city = await _unitOfWork.GetRepository<City>()
                .SingleOrDefaultAsync(
                    predicate: x => x.Id == id,
                    include: x => x.Include(x => x.Districts))
                ?? throw new BadHttpRequestException(MessageConstant.City.NotFoundFailedMessage);

            var cityResponse = new GetCityResponse
            {
                Id = city.Id,
                UnitId = city.UnitId,
                Name = city.Name,
                Status = string.IsNullOrEmpty(city.Status) ? null : EnumUtil.ParseEnum<CityStatus>(city.Status),
            };

            return cityResponse;
        }

        public async Task<ICollection<GetCityResponse>> GetCityList(CityFilter filter)
        {
            var cityList = await _unitOfWork.GetRepository<City>()
                .GetListAsync(
                    selector: x => x,
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.Name),
                    include: x => x.Include(x => x.Districts))
                ?? throw new BadHttpRequestException(MessageConstant.City.NotFoundFailedMessage);

            var cityResponses = cityList.Select(city => new GetCityResponse
            {
                Id = city.Id,
                UnitId = city.UnitId,
                Name = city.Name,
                Status = string.IsNullOrEmpty(city.Status) ? null : EnumUtil.ParseEnum<CityStatus>(city.Status),
            }).ToList();

            return cityResponses;
        }

        public async Task<bool> RemoveCityStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.City.CityEmptyMessage);
            City city = await _unitOfWork.GetRepository<City>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.News.NewsNotFoundMessage);
            city.Status = CityStatus.InActive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<City>().UpdateAsync(city);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateCity(Guid id, UpdateCityRequest updateCityRequest)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException(MessageConstant.City.CityEmptyMessage);

            var city = await _unitOfWork.GetRepository<City>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.City.CityExistedMessage);

            city.Name = string.IsNullOrEmpty(updateCityRequest.Name) ? city.Name : updateCityRequest.Name;
            
            if (!updateCityRequest.Status.HasValue)
            {
                throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
            }
            else
            {
                city.Status = updateCityRequest.Status.GetDescriptionFromEnum();
            }
            _unitOfWork.GetRepository<City>().UpdateAsync(city);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }
    }
}
