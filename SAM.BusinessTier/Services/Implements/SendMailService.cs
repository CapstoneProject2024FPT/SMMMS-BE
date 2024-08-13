//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using SAM.BusinessTier.Payload.Mail;
//using SAM.BusinessTier.Services.Interfaces;
//using SAM.DataTier.Models;
//using SAM.DataTier.Repository.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SAM.BusinessTier.Services.Implements
//{
//    public class SendMailService : BaseService<SendMailService>, ISendMailService
        
//    {
//        private readonly IConfiguration _configuration;
//        public SendMailService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<SendMailService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor)
//        {
//            _configuration = configuration;
//        }

//        public async Task SendMail(MailContent mailContent)
//        {
//            var email = new MimeMessage();
//            email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
//            email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
//            email.To.Add(MailboxAddress.Parse(mailContent.To));
//            email.Subject = mailContent.Subject;
//            var mail = _configuration["MailSettings:Mail"];

//            var builder = new BodyBuilder();
//            builder.HtmlBody = mailContent.Body;
//            email.Body = builder.ToMessageBody();

//            // dùng SmtpClient của MailKit
//            using var smtp = new MailKit.Net.Smtp.SmtpClient();

//            try
//            {
//                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
//                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
//                await smtp.SendAsync(email);
//            }
//            catch (Exception ex)
//            {
//                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
//                System.IO.Directory.CreateDirectory("mailssave");
//                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
//                await email.WriteToAsync(emailsavefile);

//                logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
//                logger.LogError(ex.Message);
//            }

//            smtp.Disconnect(true);

//            logger.LogInformation("send mail to " + mailContent.To);

//        }
//        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
//        {
//            await SendMail(new MailContent()
//            {
//                To = email,
//                Subject = subject,
//                Body = htmlMessage
//            });
//        }
//    }
//}
