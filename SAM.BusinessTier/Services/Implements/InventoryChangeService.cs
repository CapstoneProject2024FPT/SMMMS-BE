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


namespace SAM.BusinessTier.Services.Implements
{
    public class InventoryChangeService : BaseService<InventoryChangeService>, IInventoryChangeService
    {
        public InventoryChangeService(IUnitOfWork<SamContext> unitOfWork, ILogger<InventoryChangeService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<bool> DeleteInventoryChange(Guid newInventoryId)
        {
            // Fetch the InventoryChange with the specified NewInventoryId
            var inventoryChange = await _unitOfWork.GetRepository<InventoryChange>().SingleOrDefaultAsync(
                predicate: x => x.NewInventoryId.Equals(newInventoryId)
            );

            if (inventoryChange == null)
            {
                throw new BadHttpRequestException(MessageConstant.Inventory.NotFoundFailedMessage);
            }

            // Fetch the new Inventory
            var newInventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(inventoryChange.NewInventoryId));

            if (newInventory == null)
            {
                throw new BadHttpRequestException("Không tìm thấy số máy mới trong hệ thống");
            }

            // Fetch the old Inventory
            var oldInventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(inventoryChange.OldInventoryId));
            if (oldInventory == null)
            {
                throw new BadHttpRequestException("Không tìm thấy số máy cũ trong hệ thống");
            }

            newInventory.Status = InventoryStatus.Available.GetDescriptionFromEnum();
            newInventory.Condition = InventoryCondition.New.GetDescriptionFromEnum();
            newInventory.MasterInventoryId = null;

            oldInventory.Status = InventoryStatus.Sold.GetDescriptionFromEnum();
            oldInventory.IsRepaired = InventoryIsRepaired.New.GetDescriptionFromEnum(); 


            _unitOfWork.GetRepository<Inventory>().UpdateAsync(newInventory);
            _unitOfWork.GetRepository<Inventory>().UpdateAsync(oldInventory);

            // Delete the InventoryChange record
            var inventoryChangeRepo = _unitOfWork.GetRepository<InventoryChange>();
            inventoryChangeRepo.DeleteAsync(inventoryChange);

            // Commit changes to the database
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

    }
}
