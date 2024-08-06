using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Notification
{
    public class GetNotificationResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsRead { get; set; }
        public string? NotificationType { get; set; }
    }
}
