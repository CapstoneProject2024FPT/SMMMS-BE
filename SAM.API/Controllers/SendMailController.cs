using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.City;
using SAM.BusinessTier.Payload.Mail;
using SAM.BusinessTier.Services.Implements;
using SAM.BusinessTier.Services.Interfaces;

namespace SAM.API.Controllers
{
    [ApiController]
    public class SendMailController : BaseController<SendMailController>
    {
        private readonly ISendMailService _senMailService;

        public SendMailController(ILogger<SendMailController> logger, ISendMailService sendMailService) : base(logger)
        {
            _senMailService = sendMailService;
        }
        [HttpPost(ApiEndPointConstant.Mail.MailsEndPoint)]
        public async Task SendMail(string to, string subject, string body)
        {
            await _senMailService.SendMail(to, subject, body);
        }

    }
}
