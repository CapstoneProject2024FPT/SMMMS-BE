using AutoMapper;
using SAM.BusinessTier.Payload.Notification;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Mappers
{
    public class NotificationModelu : Profile
    {
        public NotificationModelu() {
            CreateMap<CreateNotificationRequest, Notification>();
            CreateMap<Notification, GetNotificationResponse>();
        }
    }
}
