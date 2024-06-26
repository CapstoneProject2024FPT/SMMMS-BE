using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.MachineryComponent;
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
    public class MachineryComponentService : BaseService<MachineryComponentService>, IMachineryComponentService
    {
        public MachineryComponentService(IUnitOfWork<SamContext> unitOfWork, ILogger<MachineryComponentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewMachineryComponents(CreateNewMachineryComponentRequest createMachineryComponentRequest)
        {
            throw new NotImplementedException();
        }

        public Task<GetMachineryComponentResponse> GetMachineryComponentById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GetMachineryComponentResponse>> GetMachineryComponentList(MachineryComponentFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveMachineryComponentStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMachineryComponent(Guid id, UpdateMachineryComponentRequest updateMachineryComponentRequest)
        {
            throw new NotImplementedException();
        }
    }
}
