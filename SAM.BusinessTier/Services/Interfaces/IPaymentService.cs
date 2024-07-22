using Microsoft.AspNetCore.Http;
using SAM.BusinessTier.Payload.Payment;
using SAM.BusinessTier.Payload.Rank;
using SAM.BusinessTier.Payload.VNPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CreatePaymentResponse> ExecutePayment(CreatePaymentRequest request);
        Task<bool> ExecuteVnPayCallback(string? status, string? transId, string? urlCallBack);

        Task<bool> UpdatePayment(Guid id, UpdatePaymentRequest updatePaymentRequest);
    }
}
