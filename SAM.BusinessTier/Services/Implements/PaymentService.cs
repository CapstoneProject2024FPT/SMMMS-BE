using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAM.API.Utils;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Payload.Payment;
using SAM.BusinessTier.Payload.VNPay;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SAM.BusinessTier.Services.Implements
{
    public class PaymentService : BaseService<PaymentService>, IPaymentService
    {
        private readonly IConfiguration _configuration;

        public PaymentService(IUnitOfWork<SamContext> unitOfWork, ILogger<PaymentService> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _configuration = configuration;
        }
        public async Task<CreatePaymentResponse> ExecutePayment(CreatePaymentRequest request)
        {
            var currentTime = TimeUtils.GetCurrentSEATime();
            var currentTimeStamp = TimeUtils.GetTimestamp(currentTime);

            var txnRef = currentTime.ToString("yyMMdd") + "_" + currentTimeStamp;
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["VnPayPaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_Amount", ((int)request.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", currentTimeStamp);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(_httpContextAccessor.HttpContext));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"Thanh toán cho đơn hàng {request.OrderId}");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", txnRef);

            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            var paymentResponse = new CreatePaymentResponse()
            {
                Message = "Đang tiến hành thanh toán VNPAY",
                Url = paymentUrl
            };

            var payment = new Payment()
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                PaymentDate = currentTime,
                PaymentMethod = request.PaymentType.GetDescriptionFromEnum(),
                OrderId = request.OrderId,
            };
            var transaction = new TransactionPayment()
            {
                Id = Guid.NewGuid(),
                PaymentId = request.PaymentId,
                InvoiceId = txnRef,
                TotalAmount = request.Amount,
                Description = $"Đang tiến hành thanh toán VNPAY mã đơn {txnRef}",
                CreatedAt = currentTime,
                PayType = request.PaymentType.GetDescriptionFromEnum(),
                Status = PaymentStatus.PENDING.GetDescriptionFromEnum()
            };

            try
            {
                await _unitOfWork.GetRepository<TransactionPayment>().InsertAsync(transaction);
                await _unitOfWork.GetRepository<Payment>().InsertAsync(payment);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return paymentResponse;
        }

        public async Task<bool> ExecuteVnPayCallback(IQueryCollection collections, string url, string? status, string? transId)
        {
            var paymentTransaction = await _unitOfWork.GetRepository<TransactionPayment>().SingleOrDefaultAsync(
                  predicate: x => x.InvoiceId.Equals(transId),
                  include: x => x.Include(x => x.Payment));

            var response = VnPayLibrary.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
            var responseJson = await CallApiUtils.CallApiEndpoint(url, response);

            if (!status.Equals("00"))
            {
                paymentTransaction.Description = "VNPAY payment failed";
                paymentTransaction.Status = "Canceled";
                paymentTransaction.Payment.Status = PaymentStatus.PAID.GetDescriptionFromEnum();

                _unitOfWork.GetRepository<TransactionPayment>().UpdateAsync(paymentTransaction);
            }
            else
            {
                paymentTransaction.Description = "VnPay payment successful";
                paymentTransaction.Status = "Completed";
                paymentTransaction.TransactionJson = responseJson.Content.ReadAsStringAsync().Result;

                _unitOfWork.GetRepository<TransactionPayment>().UpdateAsync(paymentTransaction);
            }
            return await _unitOfWork.CommitAsync() > 0;
        }
    }
}
