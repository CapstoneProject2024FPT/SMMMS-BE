using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Notification;
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
    public class DeviceService : BaseService<DeviceService>, IDeviceService
    {
        public DeviceService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<DeviceService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> RegisterDevice(DeviceRegistrationRequest request)
        {
            var currentUser = GetUsernameFromJwt();
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser))
                ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);
            DateTime currentTime = TimeUtils.GetCurrentSEATime();
            var device = new Device
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                Fcmtoken = request.FCMToken,
                LastUpdated = currentTime
            };

            await _unitOfWork.GetRepository<Device>().InsertAsync(device);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return device.Id;
        }

        public async Task<bool> UpdateDevice(Guid id, DeviceRegistrationRequest request)
        {
            var device = await _unitOfWork.GetRepository<Device>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (device == null) throw new BadHttpRequestException(MessageConstant.Device.EmptyMessage);

            device.Fcmtoken = request.FCMToken;
            device.LastUpdated = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<Device>().UpdateAsync(device);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

        public async Task<bool> RemoveDevice(string token)
        {
            var device = await _unitOfWork.GetRepository<Device>().SingleOrDefaultAsync(
                predicate: x => x.Fcmtoken.Equals(token));
            if (device == null) throw new BadHttpRequestException(MessageConstant.Device.EmptyMessage);

            _unitOfWork.GetRepository<Device>().DeleteAsync(device);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
