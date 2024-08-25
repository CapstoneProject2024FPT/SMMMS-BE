using SAM.BusinessTier.Payload.Device;
using SAM.BusinessTier.Payload.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IDeviceService
    {
        Task<Guid> RegisterDevice(DeviceRegistrationRequest request);
        Task<bool> UpdateDevice(Guid id, DeviceRegistrationRequest request);
        Task<bool> RemoveDevice(DeleteDeviceRequest deleteDeviceRequest);
    }
}
