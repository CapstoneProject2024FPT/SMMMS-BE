using SAM.BusinessTier.Payload.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface ISendMailService
    {
        Task SendMail(string to, string subject, string body);

        //Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
