using AutoMapper;
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

            if (account == null)
            {
                throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);
            }

            DateTime currentTime = TimeUtils.GetCurrentSEATime();
            Order newOrder = new()
            {
                Id = Guid.NewGuid(),
                InvoiceCode = TimeUtils.GetTimestamp(currentTime),
                CreateDate = currentTime,
                CompletedDate = null,
                TotalAmount = request.TotalAmount,
                FinalAmount = request.FinalAmount,
                Note = request.Note,
                Description = request.Description,
                Status = OrderStatus.UnPaid.GetDescriptionFromEnum(),
                AccountId = account.Id,
                AddressId = request.AddressId
            };

            var orderDetails = new List<OrderDetail>();

            foreach (var machinery in request.MachineryList)
            {
                var machineryExists = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(machinery.MachineryId));
                if (machineryExists == null)
                {
                    throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);
                }

                var inventories = await _unitOfWork.GetRepository<Inventory>().GetListAsync(
                    predicate: x => x.MachineryId == machinery.MachineryId && x.Status == InventoryStatus.Available.GetDescriptionFromEnum()
                );

                if (inventories.Count < machinery.Quantity)
                {
                    throw new BadHttpRequestException(MessageConstant.Inventory.NotAvaliable);
                }

                foreach (var inventory in inventories.Take((int)machinery.Quantity))
                {
                    inventory.Status = InventoryStatus.Pending.GetDescriptionFromEnum();
                    _unitOfWork.GetRepository<Inventory>().UpdateAsync(inventory);

                    var orderDetail = new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        OrderId = newOrder.Id,
                        MachineryId = machinery.MachineryId,
                        InventoryId = inventory.Id,
                        Quantity = 1, // Chỉnh Quantity thành 1 cho mỗi máy
                        SellingPrice = machinery.SellingPrice,
                        TotalAmount = machinery.SellingPrice, // Tổng tiền cho mỗi máy
                        CreateDate = DateTime.Now
                    };

                    orderDetails.Add(orderDetail);
                }
            }

            await _unitOfWork.GetRepository<Order>().InsertAsync(newOrder);
            await _unitOfWork.GetRepository<OrderDetail>().InsertRangeAsync(orderDetails);
            await _unitOfWork.CommitAsync();

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
                                   .ThenInclude(detail => detail.Inventory.Machinery))
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
                Description = order.Description,
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
                        UnitId = order.Address.City.UnitId,
                        Name = order.Address.City.Name
                    },
                    District = order.Address.District == null ? null : new DistrictResponse
                    {
                        Id = order.Address.District.Id,
                        UnitId = order.Address.District.UnitId,
                        Name = order.Address.District.Name
                    },
                    Ward = order.Address.Ward == null ? null : new WardResponse
                    {
                        Id = order.Address.Ward.Id,
                        UnitId = order.Address.Ward.UnitId,
                        Name = order.Address.Ward.Name
                    },
                    Account = order.Address.Account == null ? null : new AccountResponse
                    {
                        Id = order.Address.Account.Id,
                        FullName = order.Address.Account.FullName,
                        Role = EnumUtil.ParseEnum<RoleEnum>(order.Address.Account.Role),
                    }
                },
                ProductList = order.OrderDetails?.Select(detail => new OrderDetailResponse
                {
                    OrderDetailId = detail.Id,
                    InventoryId = detail.InventoryId,
                    ProductId = detail.MachineryId,
                    ProductName = detail.Inventory.Machinery?.Name,
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
                    Description = x.Description,
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
                            UnitId = x.Address.City.UnitId,
                            Name = x.Address.City.Name
                        },
                        District = x.Address.District == null ? null : new DistrictResponse
                        {
                            Id = x.Address.District.Id,
                            UnitId = x.Address.District.UnitId,
                            Name = x.Address.District.Name
                        },
                        Ward = x.Address.Ward == null ? null : new WardResponse
                        {
                            Id = x.Address.Ward.Id,
                            UnitId = x.Address.Ward.UnitId,
                            Name = x.Address.Ward.Name
                        },
                        Account = x.Address.Account == null ? null : new AccountResponse
                        {
                            Id = x.Address.Account.Id,
                            FullName = x.Address.Account.FullName,
                            Role = EnumUtil.ParseEnum<RoleEnum>(x.Address.Account.Role),
                        }
                    },
                    ProductList = x.OrderDetails.Select(detail => new OrderDetailResponse
                    {
                        OrderDetailId = detail.Id,
                        InventoryId = detail.InventoryId,
                        ProductId = detail.MachineryId,
                        ProductName = detail.Inventory.Machinery.Name,
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
                                   .ThenInclude(detail => detail.Inventory.Machinery),
                page: pagingModel.page,
                size: pagingModel.size
            );

            return orderList;
        }






        public async Task<bool> UpdateOrder(Guid orderId, UpdateOrderRequest request)
        {
            string currentUser = GetUsernameFromJwt();
            var userId = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser),
                selector: x => x.Id);
            Order updateOrder = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderId))
                ?? throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);
            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            switch (request.Status)
            {
                case OrderStatus.Completed:
                    updateOrder.Status = OrderStatus.Completed.GetDescriptionFromEnum();
                    updateOrder.CompletedDate = currentTime;

                    // Tạo Warranty khi Order hoàn thành
                    var orderDetails = await _unitOfWork.GetRepository<OrderDetail>().GetListAsync(
                        predicate: x => x.OrderId == orderId);

                    var taskManager = await _unitOfWork.GetRepository<TaskManager>().SingleOrDefaultAsync(
                        predicate: t => t.OrderId == orderId);

                    if (taskManager != null)
                    {
                        taskManager.Status = TaskManagerStatus.Completed.GetDescriptionFromEnum();
                        _unitOfWork.GetRepository<TaskManager>().UpdateAsync(taskManager);
                    }

                    break;
                case OrderStatus.Delivery:
                    updateOrder.Status = OrderStatus.Delivery.GetDescriptionFromEnum();
                    break;
                case OrderStatus.Paid:

                    var orderDetailsPaid = await _unitOfWork.GetRepository<OrderDetail>().GetListAsync(
                        predicate: x => x.OrderId == orderId);
                    foreach (var detail in orderDetailsPaid)
                    {
                        var inventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                            predicate: x => x.Id == detail.InventoryId,
                            include: i => i.Include(i => i.Machinery).ThenInclude(m => m.MachineryComponentParts).ThenInclude(cp => cp.MachineComponents));

                        if (inventory == null || inventory.Machinery == null)
                        {
                            throw new BadHttpRequestException(MessageConstant.Inventory.NotFoundFailedMessage);
                        }

                        int warrantyYears = (int)inventory.Machinery.TimeWarranty;
                        int numberOfDetails = (warrantyYears * 12) / 6; // Number of details based on 6 months intervals

                        Warranty newWarranty = new Warranty
                        {
                            Id = Guid.NewGuid(),
                            Type = WarrantyType.Periodic.GetDescriptionFromEnum(),
                            CreateDate = currentTime,
                            StartDate = currentTime,
                            Status = WarrantyStatus.AwaitingAssignment.GetDescriptionFromEnum(),
                            Description = updateOrder.Note,
                            Priority = 1,
                            InventoryId = detail.InventoryId
                        };
                        await _unitOfWork.GetRepository<Warranty>().InsertAsync(newWarranty);

                        // Tạo WarrantyDetail cho Periodic Warranty dựa trên TimeWarranty
                        if (newWarranty.Type == WarrantyType.Periodic.GetDescriptionFromEnum())
                        {
                            for (int i = 1; i <= numberOfDetails; i++)
                            {
                                DateTime maintenanceDate = currentTime.AddMonths(i * 6);

                                WarrantyDetail newWarrantyDetail = new WarrantyDetail
                                {
                                    Id = Guid.NewGuid(),
                                    Type = newWarranty.Type,
                                    CreateDate = currentTime,
                                    StartDate = maintenanceDate,
                                    Status = WarrantyDetailStatus.AwaitingAssignment.GetDescriptionFromEnum(),
                                    Description = $"Periodic Warranty Detail - Start on {maintenanceDate:yyyy-MM-dd HH:mm:ss}",
                                    WarrantyId = newWarranty.Id,
                                    AddressId = updateOrder.AddressId
                                };
                                await _unitOfWork.GetRepository<WarrantyDetail>().InsertAsync(newWarrantyDetail);
                            }
                        }
                    }
                    foreach (var detail in orderDetailsPaid)
                    {
                        var inventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                            predicate: x => x.Id == detail.InventoryId,
                            include: i => i.Include(i => i.Machinery).ThenInclude(m => m.MachineryComponentParts).ThenInclude(cp => cp.MachineComponents));

                        if (inventory != null)
                        {
                            inventory.Status = InventoryStatus.Sold.GetDescriptionFromEnum();
                            _unitOfWork.GetRepository<Inventory>().UpdateAsync(inventory);

                            // Cập nhật trạng thái cho các component của Machinery
                            foreach (var componentPart in inventory.Machinery.MachineryComponentParts)
                            {
                                var componentInventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                                    predicate: x => x.MachineComponentsId == componentPart.MachineComponentsId && x.MasterInventoryId == inventory.Id);

                                if (componentInventory != null)
                                {
                                    componentInventory.Status = InventoryStatus.Sold.GetDescriptionFromEnum();
                                    _unitOfWork.GetRepository<Inventory>().UpdateAsync(componentInventory);
                                }
                            }
                        }
                    }
                    updateOrder.Status = OrderStatus.Paid.GetDescriptionFromEnum();
                    break;
                case OrderStatus.Canceled:
                    // Cập nhật trạng thái của Inventory sang Available
                    var orderDetailsCanceled = await _unitOfWork.GetRepository<OrderDetail>().GetListAsync(
                        predicate: x => x.OrderId == orderId);
                    foreach (var detail in orderDetailsCanceled)
                    {
                        var inventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                            predicate: x => x.Id == detail.InventoryId);
                        if (inventory != null)
                        {
                            inventory.Status = InventoryStatus.Available.GetDescriptionFromEnum();
                            _unitOfWork.GetRepository<Inventory>().UpdateAsync(inventory);
                        }
                    }
                    updateOrder.Status = OrderStatus.Canceled.GetDescriptionFromEnum();
                    updateOrder.CompletedDate = currentTime;
                    break;
                default:
                    return false;
            }

            // Lưu trữ ghi chú nếu có
            if (!string.IsNullOrEmpty(request.Note))
            {
                updateOrder.Note = request.Note;
                updateOrder.Description = request.Description;
            }

            _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }


    }
}

