using AutoMapper;
using Azure.Core;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums;
using SAM.BusinessTier.Extensions;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Order;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Paginate;
using SAM.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Implements
{
    public class OrderService : BaseService<OrderService>, IOrderService
    {
        public OrderService(IUnitOfWork<SamContext> unitOfWork, ILogger<OrderService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewOrder(CreateNewOrderResponse request)
        {
            var currentUser = GetUsernameFromJwt();
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser));
            DateTime currentTime = TimeUtils.GetCurrentSEATime();
            Order newOrder = new()
            {
                Id = Guid.NewGuid(),
                InvoiceCode = TimeUtils.GetTimestamp(currentTime),
                CreateDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                TotalAmount = request.TotalAmount,
                FinalAmount = request.FinalAmount,
                Note = request.Note,
                Status = OrderStatus.PENDING.GetDescriptionFromEnum(),
                AccountId = request.AccountId
            };

            var orderDetails = new List<OrderDetail>();
            foreach (var machinery in request.MachineryList)
            {
                double totalProductAmount = (double)(machinery.SellingPrice * machinery.Quantity);
                orderDetails.Add(new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = newOrder.Id,
                    MachineryId = machinery.MachineryId,
                    Quantity = machinery.Quantity,
                    SellingPrice = machinery.SellingPrice,
                    TotalAmount = totalProductAmount
                });

            };
            //OrderHistory history = new OrderHistory()
            //{
            //    Id = Guid.NewGuid() ,
            //    Status = OrderHistoryStatus.PENDING.GetDescriptionFromEnum(),
            //    Note = request.Note,
            //    CreateDate = currentTime,
            //    OrderId = newOrder.Id,
            //    UserId = account.UserId,
            //};

            await _unitOfWork.GetRepository<Order>().InsertAsync(newOrder);
            await _unitOfWork.GetRepository<OrderDetail>().InsertRangeAsync(orderDetails);
            //await _unitOfWork.GetRepository<OrderHistory>().InsertAsync(history);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Order.CreateOrderFailedMessage);
            return newOrder.Id;
        }

        public async Task<GetOrderDetailResponse> GetOrderDetail(Guid id)
        {
            DateTime currentTime = TimeUtils.GetCurrentSEATime();
            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            var orderDetailResponse = new GetOrderDetailResponse()
            {
                OrderId = order.Id,
                InvoiceCode = order.InvoiceCode,
                CreateDate = order.CreateDate,
                CompletedDate = order.CompletedDate,
                TotalAmount = order.TotalAmount,
                FinalAmount = order.FinalAmount,
                Note = order.Note,
                Status = EnumUtil.ParseEnum<OrderStatus>(order.Status),

                ProductList = (List<OrderDetailResponse>)await _unitOfWork.GetRepository<OrderDetail>()
                    .GetListAsync(
                        selector: x => new OrderDetailResponse()
                        {
                            OrderDetailId = x.Id,
                            ProductId = x.MachineryId,
                            ProductName = x.Machinery.Name,
                            Quantity = x.Quantity,
                            SellingPrice = x.SellingPrice,  
                            TotalAmount = x.TotalAmount,
                        },
                        predicate: x => x.OrderId.Equals(id)
                    ),
                UserInfo = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    selector: x => new OrderUserResponse()
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        Role = EnumUtil.ParseEnum<RoleEnum>(x.Role)
                    },
                    predicate: x => x.Id.Equals(order.AccountId))
                
            };
            return orderDetailResponse;
        }
        //private static Expression<Func<Order, bool>> BuildGetOrderQuery(OrderFilter filter)
        //{
        //    Expression<Func<Order, bool>> filterQuery = x => true;

        //    var InvoiceCode = filter.InvoiceCode;
        //    var fromDate = filter.fromDate;
        //    var toDate = filter.toDate;
        //    var status = filter.status;

        //    if (InvoiceCode != null)
        //    {
        //        filterQuery = filterQuery.AndAlso(x => x.InvoiceCode.Contains(InvoiceCode));
        //    }

        //    if (fromDate.HasValue)
        //    {
        //        filterQuery = filterQuery.AndAlso(x => x.CreateDate >= fromDate);
        //    }

        //    if (toDate.HasValue)
        //    {
        //        filterQuery = filterQuery.AndAlso(x => x.CreateDate <= toDate);
        //    }

        //    if (status != null)
        //    {
        //        filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
        //    }


        //    return filterQuery;
        //}


        public async Task<IPaginate<GetOrderResponse>> GetOrderList(OrderFilter filter, PagingModel pagingModel)
        {
            IPaginate<GetOrderResponse> response = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                selector: x => _mapper.Map<GetOrderResponse>(x),
                filter: filter,
                orderBy: x => x.OrderByDescending(x => x.CreateDate),
                page: pagingModel.page,
                size: pagingModel.size
                );
            return response;
        }

        public async Task<bool> UpdateOrder(Guid orderId, UpdateOrderRequest request)
        {
            string currentUser = GetUsernameFromJwt();
            var userId = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate : x => x.Username.Equals(currentUser),
                selector: x => x.Id); 
            Order updateOrder = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderId))
                ?? throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);
            DateTime currentTime = TimeUtils.GetCurrentSEATime();
            switch (request.Status)
            {
                case OrderStatus.COMPLETED:
                    break;
                case OrderStatus.CONFIRMED:
                    break;
                case OrderStatus.PAID:
                    break;
                case OrderStatus.CANCELED:
                    updateOrder.Status = OrderStatus.CANCELED.GetDescriptionFromEnum();
                    _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
                    
                    break;
                default:
                    return false;
            }
            //OrderHistory history = new OrderHistory()
            //{
            //    Id = Guid.NewGuid(),
            //    Status = request.Status.GetDescriptionFromEnum(),
            //    Note = request.Note,
            //    CreateDate = currentTime,
            //    OrderId = orderId,
            //    UserId = userId,
            //};
            //await _unitOfWork.GetRepository<OrderHistory>().InsertAsync(history);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        //public async Task<IEnumerable<GetOrderHistoriesResponse>> GetOrderHistories(Guid orderId)
        //{
        //    IEnumerable<GetOrderHistoriesResponse> respone = await _unitOfWork.GetRepository<OrderHistory>().GetListAsync(
        //       selector: x => _mapper.Map<GetOrderHistoriesResponse>(x),
        //       include: x => x.Include(x => x.User),
        //       orderBy: x => x.OrderByDescending(x => x.CreateDate));
        //    return respone;
        //}
    }
}

