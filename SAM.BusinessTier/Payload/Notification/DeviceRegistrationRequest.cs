using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Notification
{
    public class DeviceRegistrationRequest
    {
        public Guid? AccountId { get; set; }
        public string? FCMToken { get; set; }
        public string? DeviceType { get; set; }
    }
}
