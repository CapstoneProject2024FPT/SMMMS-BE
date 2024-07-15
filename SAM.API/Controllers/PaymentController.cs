using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Payment;
using SAM.BusinessTier.Payload.VNPay;
using SAM.BusinessTier.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SAM.API.Controllers
{
    [ApiController]
    public class PaymentController : BaseController<PaymentController>
    {
        readonly IPaymentService _paymentService;

        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService) : base(logger)
        {

            _paymentService = paymentService;
        }
        [HttpPost(ApiEndPointConstant.Payment.PaymentEndpoint)]
        [ProducesResponseType(typeof(CreatePaymentResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateVnPayPaymentUrl([FromBody] CreatePaymentRequest createPaymentRequest)
        {
            var url = await _paymentService.ExecutePayment(createPaymentRequest);
            return Ok(url);
        }

        [HttpGet(ApiEndPointConstant.Payment.VnPayEndpoint)]
        public async Task<IActionResult> VnPayPaymentCallBack([Required] string url, string? vnp_ResponseCode, string? vnp_TxnRef)
        {
            var isSuccessful = await _paymentService.ExecuteVnPayCallback(Request.Query, url, vnp_ResponseCode, vnp_TxnRef);

            if (isSuccessful && vnp_ResponseCode == "00")
            {
                // Payment success redirect
                return RedirectToAction("PaymentSuccessAction", "PaymentController");
            }
            else
            {
                // Payment failure redirect
                return RedirectToAction("PaymentFailureAction", "PaymentController");
            }
        }

    }
}
