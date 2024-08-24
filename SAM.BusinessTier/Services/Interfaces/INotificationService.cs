using SAM.BusinessTier.Payload.Notification;
using SAM.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendPushNotificationAsync(string title, string body, Guid AccountId);
        //Task<Guid> CreateNotification(CreateNotificationRequest request);
        //Task<GetNotificationResponse> GetNotification(Guid id);
        //Task<ICollection<GetNotificationResponse>> GetNotifications(NotificationFilter filter);
        //Task<bool> MarkNotificationAsRead(Guid id);
        //Task<bool> RemoveNotification(Guid id);
        //Task<bool> UpdateNotification(Guid id, CreateNotificationRequest request);
    }
}
