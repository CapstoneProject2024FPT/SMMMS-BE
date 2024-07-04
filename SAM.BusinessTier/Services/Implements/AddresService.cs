using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Address;
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
    public class AddresService : BaseService<AddresService>, IAddressService
    {
        public AddresService(IUnitOfWork<SamContext> unitOfWork, ILogger<AddresService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewAddress(CreateNewAddresRequest createNewAddresRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetAddressResponse>> GetAddressList(AddressFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<GetAddressResponse> GetBrandById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAddressStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAddress(Guid id, UpdateAddressRequest updateAddressRequest)
        {
            throw new NotImplementedException();
        }
    }
}
