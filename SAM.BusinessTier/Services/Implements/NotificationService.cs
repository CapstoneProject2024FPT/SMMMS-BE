using AutoMapper;
using ExpoCommunityNotificationServer.Client;
using ExpoCommunityNotificationServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Notification;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Implements
{
    public class NotificationService : BaseService<NotificationService>, INotificationService
    {

        public NotificationService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<NotificationService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task SendPushNotificationAsync(string title, string body, Guid AccountId)
        {
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(AccountId))
                ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);

            var deviceIds = (await _unitOfWork.GetRepository<Device>()
                .FindAsync(
                    expression: token => token.AccountId == account.Id))
                .Select(_ => _.Fcmtoken).ToList();

            if (deviceIds == null || !deviceIds.Any())
            {
                _logger.LogWarning($"No device tokens found for user {account.Id}. Notification will not be sent.");
                return;
            }

            if (deviceIds == null || !deviceIds.Any())
            {
                _logger.LogWarning($"No device tokens found for user {account.Id}. Notification will not be sent.");
                return;
            }

            IPushApiClient _client = new PushApiClient("ehAXa94NsN6NnpSTLLZkb2vnmxZC3Y-vF0k7xDkk");
            foreach (var deviceId in deviceIds)
            {
                PushTicketRequest pushTicketRequest = new PushTicketRequest()
                {
                    PushTo = new List<string> { deviceId },
                    PushTitle = title,
                    PushBody = body,
            //        PushData = new Dictionary<string, object>()
            //{
            //    { "key1", "value1" },
            //    { "key2", "value2" }
            //}
                };

                _logger.LogInformation($"[Expo NOTIFICATION] Data full: {JsonSerializer.Serialize(pushTicketRequest)}");
                _logger.LogInformation($"[Expo NOTIFICATION] Data: {JsonSerializer.Serialize(pushTicketRequest.PushData)}");

                try
                {
                    PushTicketResponse result = await _client.SendPushAsync(pushTicketRequest);
                    _logger.LogInformation($"[Expo NOTIFICATION] Success push notification: {JsonSerializer.Serialize(result)}");
                }
                catch (ExpoCommunityNotificationServer.Exceptions.InvalidRequestException ex)
                {
                    _logger.LogError(ex, "Invalid request: {Message}", ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while sending notification");
                }
            }
        }
    }

}
