using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.Other;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SAM.BusinessTier.Constants.MessageConstant;


namespace SAM.BusinessTier.Services.Implements
{
    public class ComponentChangeService : BaseService<ComponentChangeService>, IComponentChangeService
    {
        public ComponentChangeService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<ComponentChangeService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<bool> DeleteComponentChange(Guid warrantyDetailId)
        {
            var componentChange = _unitOfWork.GetRepository<ComponentChange>();

            var component = await componentChange.SingleOrDefaultAsync(
            predicate: c => c.WarrantyDetailId == warrantyDetailId);

            if (componentChange == null)
            {
                throw new BadHttpRequestException(MessageConstant.MachineryComponents.MachineryComponentsNotFoundMessage);
            }

            componentChange.DeleteAsync(component);

            var isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }


    }
}
