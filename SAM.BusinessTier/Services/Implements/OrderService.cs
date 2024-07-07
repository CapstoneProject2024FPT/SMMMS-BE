﻿using AutoMapper;
using Azure.Core;
using SAM.BusinessTier.Constants;
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
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.Address;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Payload.News;
using SAM.BusinessTier.Payload.Wards;

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
                Status = OrderStatus.Pending.GetDescriptionFromEnum(),
                AccountId = request.AccountId,
                AddressId = request.AddressId
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
                    TotalAmount = totalProductAmount,
                    CreateDate = DateTime.Now
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
            // Fetch order including related entities from the database
            var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Account)
                               .Include(x => x.Address)
                                   .ThenInclude(a => a.City)
                               .Include(x => x.Address)
                                   .ThenInclude(a => a.District)
                               .Include(x => x.Address)
                                   .ThenInclude(a => a.Ward)
                               .Include(x => x.Address)
                                   .ThenInclude(a => a.Account)
                               .Include(x => x.OrderDetails)
                                   .ThenInclude(detail => detail.Machinery))
                ?? throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            // Map fetched data to the response DTO
            var getOrderDetailResponse = new GetOrderDetailResponse
            {
                OrderId = order.Id,
                InvoiceCode = order.InvoiceCode,
                CreateDate = order.CreateDate,
                CompletedDate = order.CompletedDate,
                TotalAmount = order.TotalAmount,
                FinalAmount = order.FinalAmount,
                Note = order.Note,
                Status = EnumUtil.ParseEnum<OrderStatus>(order.Status),
                UserInfo = order.Account == null ? null : new OrderUserResponse
                {
                    Id = order.Account.Id,
                    FullName = order.Account.FullName,
                    Role = EnumUtil.ParseEnum<RoleEnum>(order.Account.Role)
                },
                Address = order.Address == null ? null : new GetAddressResponse
                {
                    Id = order.Address.Id,
                    Name = order.Address.Name,
                    Status = EnumUtil.ParseEnum<AddressStatus>(order.Address.Status),
                    Note = order.Address.Note,
                    City = order.Address.City == null ? null : new CityResponse
                    {
                        Id = order.Address.City.Id,
                        Name = order.Address.City.Name
                    },
                    District = order.Address.District == null ? null : new DistrictResponse
                    {
                        Id = order.Address.District.Id,
                        Name = order.Address.District.Name
                    },
                    Ward = order.Address.Ward == null ? null : new WardResponse
                    {
                        Id = order.Address.Ward.Id,
                        Name = order.Address.Ward.Name
                    },
                    Account = order.Address.Account == null ? null : new AccountResponse
                    {
                        Id = order.Address.Account.Id,
                        FullName = order.Address.Account.FullName
                    }
                },
                ProductList = order.OrderDetails?.Select(detail => new OrderDetailResponse
                {
                    OrderDetailId = detail.Id,
                    ProductId = detail.MachineryId,
                    ProductName = detail.Machinery?.Name,
                    Quantity = detail.Quantity,
                    SellingPrice = detail.SellingPrice,
                    TotalAmount = detail.TotalAmount,
                    CreateDate = detail.CreateDate,
                }).ToList() ?? new List<OrderDetailResponse>()
            };

            return getOrderDetailResponse;
        }





        public async Task<IPaginate<GetOrderDetailResponse>> GetOrderList(OrderFilter filter, PagingModel pagingModel)
        {
            IPaginate<GetOrderDetailResponse> orderList = await _unitOfWork.GetRepository<Order>().GetPagingListAsync
            (
                selector: x => new GetOrderDetailResponse
                {
                    OrderId = x.Id,
                    InvoiceCode = x.InvoiceCode,
                    CreateDate = x.CreateDate,
                    CompletedDate = x.CompletedDate,
                    TotalAmount = x.TotalAmount,
                    FinalAmount = x.FinalAmount,
                    Note = x.Note,
                    Status = EnumUtil.ParseEnum<OrderStatus>(x.Status),
                    UserInfo = x.Account == null ? null : new OrderUserResponse
                    {
                        Id = x.Account.Id,
                        FullName = x.Account.FullName,
                        Role = EnumUtil.ParseEnum<RoleEnum>(x.Account.Role)
                    },
                    Address = x.Address == null ? null : new GetAddressResponse
                    {
                        Id = x.Address.Id,
                        Name = x.Address.Name,
                        Status = EnumUtil.ParseEnum<AddressStatus>(x.Address.Status),
                        Note = x.Address.Note,
                        City = x.Address.City == null ? null : new CityResponse
                        {
                            Id = x.Address.City.Id,
                            Name = x.Address.City.Name
                        },
                        District = x.Address.District == null ? null : new DistrictResponse
                        {
                            Id = x.Address.District.Id,
                            Name = x.Address.District.Name
                        },
                        Ward = x.Address.Ward == null ? null : new WardResponse
                        {
                            Id = x.Address.Ward.Id,
                            Name = x.Address.Ward.Name
                        },
                        Account = x.Address.Account == null ? null : new AccountResponse
                        {
                            Id = x.Address.Account.Id,
                            FullName = x.Address.Account.FullName
                        }
                    },
                    ProductList = x.OrderDetails.Select(detail => new OrderDetailResponse
                    {
                        OrderDetailId = detail.Id,
                        ProductId = detail.MachineryId,
                        ProductName = detail.Machinery.Name,
                        Quantity = detail.Quantity,
                        SellingPrice = detail.SellingPrice,
                        TotalAmount = detail.TotalAmount,
                        CreateDate = detail.CreateDate,
                    }).ToList() ?? new List<OrderDetailResponse>()
                },
                filter: filter,
                orderBy: x => x.OrderByDescending(x => x.CreateDate),
                include: x => x.Include(x => x.Account)
                               .Include(x => x.Address)
                                   .ThenInclude(a => a.City)
                               .Include(x => x.Address)
                                   .ThenInclude(a => a.District)
                               .Include(x => x.Address)
                                   .ThenInclude(a => a.Ward)
                               .Include(x => x.Address)
                                   .ThenInclude(a => a.Account)
                               .Include(x => x.OrderDetails)
                                   .ThenInclude(detail => detail.Machinery),
                page: pagingModel.page,
                size: pagingModel.size
            );

            return orderList;
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
                case OrderStatus.Completed:
                    break;
                case OrderStatus.Confirmed:
                    break;
                case OrderStatus.Paid:
                    break;
                case OrderStatus.Canceled:
                    updateOrder.Status = OrderStatus.Canceled.GetDescriptionFromEnum();
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

    }
}

