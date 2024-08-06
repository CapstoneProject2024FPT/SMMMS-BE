using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Notification
{
    public class NotificationRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string FCMToken { get; set; }
    }
}
