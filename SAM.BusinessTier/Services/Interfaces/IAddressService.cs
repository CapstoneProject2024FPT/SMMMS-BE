using SAM.BusinessTier.Payload.Address;
using SAM.BusinessTier.Payload.Brand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IAddressService
    {
        Task<Guid> CreateNewAddress(CreateNewAddresRequest createNewAddresRequest);
        Task<bool> UpdateAddress(Guid id, UpdateAddressRequest updateAddressRequest);
        Task<ICollection<GetAddressResponse>> GetAddressList(AddressFilter filter);
        Task<GetAddressResponse> GetBrandById(Guid id);
        Task<bool> RemoveAddressStatus(Guid id);
    }
}
