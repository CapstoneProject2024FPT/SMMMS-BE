//using DentalLabManagement.API.Controllers;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using SAM.BusinessTier.Constants;
//using SAM.BusinessTier.Payload.Notification;
//using SAM.BusinessTier.Services.Interfaces;

//namespace SAM.API.Controllers
//{
//    [ApiController]
//    public class NotificationController : BaseController<NotificationController>
//    {
//        private readonly INotificationService _notificationService;

//        public NotificationController(ILogger<NotificationController> logger, INotificationService notificationService) : base(logger)
//        {
//            _notificationService = notificationService;
//        }

//        [HttpPost(ApiEndPointConstant.Notification.NotificationsEndPoint)]
//        public async Task<IActionResult> CreateNotification(CreateNotificationRequest createNotificationRequest)
//        {
//            var response = await _notificationService.CreateNotification(createNotificationRequest);
//            return Ok(response);
//        }

//        [HttpGet(ApiEndPointConstant.Notification.NotificationsEndPoint)]
//        public async Task<IActionResult> GetNotifications([FromQuery] NotificationFilter filter)
//        {
//            var response = await _notificationService.GetNotifications(filter);
//            return Ok(response);
//        }

//        [HttpGet(ApiEndPointConstant.Notification.NotificationEndPoint)]
//        public async Task<IActionResult> GetNotificationById(Guid id)
//        {
//            var response = await _notificationService.GetNotification(id);
//            return Ok(response);
//        }

//        [HttpPut(ApiEndPointConstant.Notification.NotificationEndPoint)]
//        public async Task<IActionResult> UpdateNotification(Guid id, CreateNotificationRequest updateNotificationRequest)
//        {
//            var isSuccessful = await _notificationService.UpdateNotification(id, updateNotificationRequest);
//            if (!isSuccessful) return Ok(MessageConstant.Notification.UpdateNotificationFailedMessage);
//            return Ok(MessageConstant.Notification.UpdateNotificationSuccessMessage);
//        }

//        [HttpPut(ApiEndPointConstant.Notification.NotificationReadEndPoint)]
//        public async Task<IActionResult> MarkNotificationAsRead(Guid id)
//        {
//            var isSuccessful = await _notificationService.MarkNotificationAsRead(id);
//            if (!isSuccessful) return Ok(MessageConstant.Notification.MarkNotificationAsReadFailedMessage);
//            return Ok(MessageConstant.Notification.MarkNotificationAsReadSuccessMessage);
//        }

//        [HttpDelete(ApiEndPointConstant.Notification.NotificationEndPoint)]
//        public async Task<IActionResult> RemoveNotification(Guid id)
//        {
//            var isSuccessful = await _notificationService.RemoveNotification(id);
//            if (!isSuccessful) return Ok(MessageConstant.Notification.RemoveNotificationFailedMessage);
//            return Ok(MessageConstant.Notification.RemoveNotificationSuccessMessage);
//        }
//    }
//}
