using DentalLabManagement.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Payment;
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
        [HttpPut(ApiEndPointConstant.Payment.PaymentsEndpoint)]
        [ProducesResponseType(typeof(CreatePaymentResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePayment(Guid id, [FromBody] UpdatePaymentRequest createPaymentRequest)
        {
            var url = await _paymentService.UpdatePayment(id, createPaymentRequest);
            return Ok(url);
        }

        [HttpGet(ApiEndPointConstant.Payment.VnPayEndpoint)]
        public async Task<IActionResult> VnPayPaymentCallBack(string? vnp_ResponseCode, string? vnp_TxnRef,string? urlCallBack)
        {
            var isSuccessful = await _paymentService.ExecuteVnPayCallback(vnp_ResponseCode, vnp_TxnRef, urlCallBack);

            if (isSuccessful && vnp_ResponseCode == "00")
            {
                return RedirectPermanent(urlCallBack);
            }
            else
            {
                return RedirectPermanent(urlCallBack);
            }
        }

    }
}