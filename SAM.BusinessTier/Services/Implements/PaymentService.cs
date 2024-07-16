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
            var urlCallBack = !string.IsNullOrEmpty(request.CallbackUrl) ? request.CallbackUrl : _configuration["VnPayPaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_Amount", ((double)request.Amount).ToString());
            pay.AddRequestData("vnp_CreateDate", currentTimeStamp);
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(_httpContextAccessor.HttpContext));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"Thanh toán cho đơn hàng {request.OrderId}");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", txnRef);

            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

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
                PaymentId = payment.Id,
                InvoiceId = txnRef,
                TotalAmount = request.Amount,
                Description = $"Đang tiến hành thanh toán VNPAY mã đơn {txnRef}",
                CreatedAt = currentTime,
                PayType = request.PaymentType.GetDescriptionFromEnum(),
                Status = PaymentStatus.PENDING.GetDescriptionFromEnum()
            };
            var paymentResponse = new CreatePaymentResponse()
            {
                Message = "Đang tiến hành thanh toán VNPAY",
                Url = paymentUrl,
                PaymentId = payment.Id

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
                paymentTransaction.Payment.Status = PaymentStatus.FAILED.GetDescriptionFromEnum();

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

        public async Task<bool> UpdatePayment(Guid id, UpdatePaymentRequest updatePaymentRequest)
        {
            // Retrieve the current user
            string currentUser = GetUsernameFromJwt();
            var userId = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser),
                selector: x => x.Id);

            // Retrieve the payment by ID
            var payment = await _unitOfWork.GetRepository<Payment>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException("Payment not found");

            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            // Update the payment status based on the request
            if (updatePaymentRequest.Status.HasValue)
            {
                payment.Status = updatePaymentRequest.Status.Value.GetDescriptionFromEnum();
            }
            else
            {
                return false;
            }

            // Save the payment note if provided
            if (!string.IsNullOrEmpty(updatePaymentRequest.Note))
            {
                payment.Note = updatePaymentRequest.Note;
            }

            // Retrieve the associated transaction
            var transaction = await _unitOfWork.GetRepository<TransactionPayment>().SingleOrDefaultAsync(
                predicate: x => x.PaymentId.Equals(payment.Id))
                ?? throw new BadHttpRequestException("Associated transaction not found");

            // Update the transaction status based on the payment status
            transaction.Status = updatePaymentRequest.Status.GetDescriptionFromEnum();

            // Update the repositories
            _unitOfWork.GetRepository<Payment>().UpdateAsync(payment);
            _unitOfWork.GetRepository<TransactionPayment>().UpdateAsync(transaction);

            // Commit the changes
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }



    }
}
