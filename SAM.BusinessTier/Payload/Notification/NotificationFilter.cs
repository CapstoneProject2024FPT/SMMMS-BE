using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Notification
{
    public class NotificationFilter
    {
        public Guid? AccountId { get; set; }
        public bool? IsRead { get; set; }
        public string NotificationType { get; set; }
    }
}
