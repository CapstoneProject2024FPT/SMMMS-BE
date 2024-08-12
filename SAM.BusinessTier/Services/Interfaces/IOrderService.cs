
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.Order;
using SAM.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateNewOrder(CreateNewOrderResquest createNewOrderRequest);
        Task<GetOrderDetailResponse> GetOrderDetail(Guid id);
        Task<IPaginate<GetOrderDetailResponse>> GetOrderList(OrderFilter filter, PagingModel pagingModel);
        Task<bool> UpdateOrder(Guid orderId, UpdateOrderRequest request);

        //Task<IEnumerable<GetOrderHistoriesResponse>> GetOrderHistories(Guid orderId);
    }
}
