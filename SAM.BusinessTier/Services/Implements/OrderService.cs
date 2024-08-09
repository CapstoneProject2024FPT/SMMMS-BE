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
        private readonly IUserService _accountService;
        public OrderService(IUnitOfWork<SamContext> unitOfWork, ILogger<OrderService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IUserService accountService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {

        }

        public async Task<Guid> CreateNewOrder(CreateNewOrderResponse request)
        {
            var currentUser = GetUsernameFromJwt();
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser))
                ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);

            DateTime currentTime = TimeUtils.GetCurrentSEATime();
            Order newOrder = new()
            {
                Id = Guid.NewGuid(),
                InvoiceCode = TimeUtils.GetTimestamp(currentTime),
                CreateDate = currentTime,
                CompletedDate = null,
                TotalAmount = 0,
                Description = request.Description,
                Status = OrderStatus.UnPaid.GetDescriptionFromEnum(),
                Type = OrderType.Order.GetDescriptionFromEnum(),
                AccountId = account.Id,
                AddressId = request.AddressId
            };

            var orderDetails = new List<OrderDetail>();
            double totalAmount = 0;

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
                        Quantity = 1,
                        SellingPrice = machinery.StockPrice,
                        TotalAmount = machinery.SellingPrice,
                        CreateDate = DateTime.Now
                    };

                    orderDetails.Add(orderDetail);
                    totalAmount += orderDetail.TotalAmount ?? 0;
                }
            }

            newOrder.TotalAmount = totalAmount;
            newOrder.FinalAmount = totalAmount;

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
                                   .ThenInclude(detail => detail.Inventory.Machinery)
                               .Include(x => x.Notes))
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
                Description = order.Description,
                Status = EnumUtil.ParseEnum<OrderStatus>(order.Status),
                Type = EnumUtil.ParseEnum<OrderType>(order.Type),
                NoteStatus = order.Notes.CountNoteEachStatus(),
                Note = order.Notes?.Select(note => new NoteResponse
                {
                    Id = note.Id,
                    Status = EnumUtil.ParseEnum<NoteStatus>(note.Status),
                    Description = note.Description,
                    CreateDate = note.CreateDate.Value,
                    
                }).ToList(),
                UserInfo = order.Account == null ? null : new OrderUserResponse
                {
                    Id = order.Account.Id,
                    FullName = order.Account.FullName,
                    Role = EnumUtil.ParseEnum<RoleEnum>(order.Account.Role)
                },
                Address = new GetAddressResponse
                {
                    Id = order.Address.Id,
                    Name = order.Address.Name,
                    Status = EnumUtil.ParseEnum<AddressStatus>(order.Address.Status),
                    Note = order.Address.Note,
                    NamePersonal = order.Address.NamePersonal,
                    PhoneNumber = order.Address.PhoneNumber,
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
            IPaginate<GetOrderDetailResponse> orderList = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                selector: x => new GetOrderDetailResponse
                {
                    OrderId = x.Id,
                    InvoiceCode = x.InvoiceCode,
                    CreateDate = x.CreateDate,
                    CompletedDate = x.CompletedDate,
                    TotalAmount = x.TotalAmount,
                    FinalAmount = x.FinalAmount,
                    Description = x.Description,
                    Status = EnumUtil.ParseEnum<OrderStatus>(x.Status),
                    NoteStatus = x.Notes.CountNoteEachStatus(),
                    Type = EnumUtil.ParseEnum<OrderType>(x.Type),
                    Note = x.Notes.Select(note => new NoteResponse
                    {
                        Id = note.Id,
                        Status = EnumUtil.ParseEnum<NoteStatus>(note.Status),
                        Description = note.Description,
                        CreateDate = note.CreateDate.Value,
                    }).ToList(),
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
                        NamePersonal = x.Address.NamePersonal.ToString(),
                        PhoneNumber = x.Address.PhoneNumber.ToString(),
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
                                   .ThenInclude(detail => detail.Inventory.Machinery)
                               .Include(x => x.Notes),
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
            if(updateOrder.Status == OrderStatus.Completed.GetDescriptionFromEnum())
            {
                throw new BadHttpRequestException("Không thể cập nhật đơn hàng khi đã completed");
            }
            switch (request.Status)
            {

                case OrderStatus.Completed:
                    updateOrder.Status = OrderStatus.Completed.GetDescriptionFromEnum();
                    updateOrder.CompletedDate = currentTime;

                    // Calculate points and update account
                    int points = (int)(updateOrder.FinalAmount / 100000);
                    var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                        predicate: x => x.Id.Equals(updateOrder.AccountId))
                        ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);

                    // Update points and save account
                    account.Point += points;
                    if (account.Point != null)
                    {
                        account.Point += points;
                    }
                    else
                    {
                        account.Point = points;
                    }
                    _unitOfWork.GetRepository<Account>().UpdateAsync(account);

                    // Lấy danh sách các rank
                    var ranks = await _unitOfWork.GetRepository<Rank>().GetListAsync(
                        predicate : x => points >= x.Range,
                        orderBy : x => x.OrderByDescending(x => x.Range)
                        );

                    var rankCheck = ranks.FirstOrDefault();
                    // Kiểm tra kết quả và thêm rank cho account nếu có
                    if (rankCheck != null)
                    {
                        var addRankResult = await _accountService.AddrankForAccount(account.Id, rankCheck.Id);
                    }


                    var taskManager = await _unitOfWork.GetRepository<TaskManager>().SingleOrDefaultAsync(
                        predicate: t => t.OrderId == orderId);

                    if (taskManager != null)
                    {
                        taskManager.Status = TaskManagerStatus.Completed.GetDescriptionFromEnum();
                        taskManager.CompletedDate = currentTime;
                        _unitOfWork.GetRepository<TaskManager>().UpdateAsync(taskManager);
                    }
                    Note note = new Note()
                    {
                        Id = Guid.NewGuid(),
                        Status = NoteStatus.SUCCESS.GetDescriptionFromEnum(),
                        CreateDate = currentTime,
                        Description = request.Note,
                        OrderId = updateOrder.Id

                    };
                    if (note != null)
                    {
                        await _unitOfWork.GetRepository<Note>().InsertAsync(note);
                    }
                    break;
                    
                case OrderStatus.Delivery:
                    updateOrder.Status = OrderStatus.Delivery.GetDescriptionFromEnum();
                    break;
                case OrderStatus.ReDelivery:

                    var taskManagers = await _unitOfWork.GetRepository<TaskManager>().GetListAsync(
                        predicate: t => t.OrderId == orderId,
                        orderBy: x => x.OrderByDescending(t => t.CreateDate)); 

                    var taskManager1 = taskManagers.FirstOrDefault();
                    if (taskManager1 != null)
                    {
                        taskManager1.Status = TaskManagerStatus.Completed.GetDescriptionFromEnum();
                        _unitOfWork.GetRepository<TaskManager>().UpdateAsync(taskManager1);
                    }
                    note = new Note()
                    {
                        Id = Guid.NewGuid(),
                        Status = NoteStatus.FAILED.GetDescriptionFromEnum(),
                        CreateDate = currentTime,
                        Description = request.Note,
                        OrderId = updateOrder.Id

                    };
                    if (note != null)
                    {
                        await _unitOfWork.GetRepository<Note>().InsertAsync(note);
                    }

                    updateOrder.Status = OrderStatus.ReDelivery.GetDescriptionFromEnum();
                    break;

                case OrderStatus.Paid:
                    if (updateOrder.Type == OrderType.Warranty.GetDescriptionFromEnum()) {
                        throw new BadHttpRequestException(MessageConstant.Transaction.CreateTransactionSuccessMessage);
                    }
                    var orderDetailsPaid = await _unitOfWork.GetRepository<OrderDetail>().GetListAsync(
                        predicate: x => x.OrderId == orderId);
                    
                    foreach (var detail in orderDetailsPaid)
                    {
                        var inventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                            predicate: x => x.Id == detail.InventoryId,
                            include: i => i.Include(i => i.Machinery)
                                            .ThenInclude(m => m.MachineryComponentParts)
                                            .ThenInclude(cp => cp.MachineComponents));

                        if (inventory == null || inventory.Machinery == null)
                        {
                            throw new BadHttpRequestException(MessageConstant.Inventory.NotFoundFailedMessage);
                        }

                        int warrantyYears = (int)inventory.Machinery.TimeWarranty;
                        int numberOfDetails = (warrantyYears * 12) / (int)inventory.Machinery.MonthWarrantyNumber;

                        Warranty newWarranty = new Warranty
                        {
                            Id = Guid.NewGuid(),
                            Type = WarrantyType.Periodic.GetDescriptionFromEnum(),
                            CreateDate = currentTime,
                            StartDate = currentTime,
                            Status = WarrantyStatus.Process.GetDescriptionFromEnum(),
                            Description = "số lần máy được bảo trì là" + numberOfDetails,
                            Priority = 1,
                            InventoryId = detail.InventoryId,
                            AccountId = updateOrder.AccountId,
                            AddressId = updateOrder.AddressId,
                        };
                        await _unitOfWork.GetRepository<Warranty>().InsertAsync(newWarranty);

                        // Create WarrantyDetail for Periodic Warranty based on TimeWarranty
                        if (newWarranty.Type == WarrantyType.Periodic.GetDescriptionFromEnum())
                        {
                            for (int i = 1; i <= numberOfDetails; i++)
                            {
                                DateTime maintenanceDate = currentTime.AddMonths(i * 6);
                                if (maintenanceDate.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    maintenanceDate = maintenanceDate.AddDays(2); // Move to Monday
                                }
                                else if (maintenanceDate.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    maintenanceDate = maintenanceDate.AddDays(1); // Move to Monday
                                }
                                WarrantyDetail newWarrantyDetail = new WarrantyDetail
                                {
                                    Id = Guid.NewGuid(),
                                    Type = newWarranty.Type,
                                    CreateDate = currentTime,
                                    StartDate = maintenanceDate,
                                    Status = WarrantyDetailStatus.AwaitingAssignment.GetDescriptionFromEnum(),
                                    Description = $"Thời gian bảo trì định kỳ bắt đầu từ {maintenanceDate:yyyy-MM-dd HH:mm:ss}",
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

                            // Update status for components of Machinery
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
                    // Update Inventory status to Available
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
                    note = new Note()
                    {
                        Id = Guid.NewGuid(),
                        Status = NoteStatus.FAILED.GetDescriptionFromEnum(),
                        CreateDate = currentTime,
                        Description = request.Note, 
                        OrderId = updateOrder.Id

                    };
                    if (note != null)
                    {
                        await _unitOfWork.GetRepository<Note>().InsertAsync(note);
                    }

                    updateOrder.Status = OrderStatus.Canceled.GetDescriptionFromEnum();
                    updateOrder.CompletedDate = currentTime;
                    break;
                default:
                    return false;
            }

            // Save notes if any
            if (!string.IsNullOrEmpty(request.Note))
            {
                updateOrder.Description = request.Description;
            }

            _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }





    }
}

