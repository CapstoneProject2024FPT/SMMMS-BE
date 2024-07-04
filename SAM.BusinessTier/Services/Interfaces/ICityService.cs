using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface ICityService
    {
        Task<Guid> CreateNewCity(CreateNewCityRequest createNewCityRequest);
        Task<bool> UpdateCity(Guid id, UpdateCityRequest updateCityRequest);
        Task<ICollection<GetCityResponse>> GetCityList(CityFilter filter);
        Task<GetCityResponse> GetCityById(Guid id);
        Task<bool> RemoveCityStatus(Guid id);
    }
}
