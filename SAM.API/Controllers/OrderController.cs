using DentalLabManagement.API.Controllers;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.Order;
using SAM.BusinessTier.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SAM.API.Controllers
{
    [ApiController]
    public class OrderController : BaseController<OrderController>
    {
        readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService) : base(logger)
        {
            _orderService = orderService;
        }
        [HttpPost(ApiEndPointConstant.Order.OrdersEndPoint)]
        public async Task<IActionResult> CreateNewOrder(CreateNewOrderResponse order)
        {
            var response = await _orderService.CreateNewOrder(order);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Order.OrderEndPoint)]
        [ProducesResponseType(typeof(GetOrderDetailResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderDetail(Guid id)
        {
            var response = await _orderService.GetOrderDetail(id);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Order.OrdersEndPoint)]
        [ProducesResponseType(typeof(GetOrderResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderList([FromQuery] OrderFilter filter, [FromQuery] PagingModel pagingModel)
        {
            var response = await _orderService.GetOrderList(filter, pagingModel);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Order.OrderEndPoint)]
        [ProducesResponseType(typeof(GetOrderDetailResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateOrder(Guid id, UpdateOrderRequest request)
        {
            var isSuccessful = await _orderService.UpdateOrder(id, request);
            if (!isSuccessful) return Ok(MessageConstant.Order.UpdateFailedMessage);
            return Ok(MessageConstant.Order.UpdateSuccessMessage);
        }
        //[HttpGet(ApiEndPointConstant.Order.OrderHistoriesEndPoint)]
        //[ProducesResponseType(typeof(GetOrderHistoriesResponse), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetOrderHistories(Guid id)
        //{
        //    var response = await _orderService.GetOrderHistories(id);
        //    return Ok(response);
        //}
    }
}
