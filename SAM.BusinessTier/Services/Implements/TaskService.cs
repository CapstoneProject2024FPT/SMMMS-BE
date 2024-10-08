﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Address;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Payload.News;
using SAM.BusinessTier.Payload.Notification;
using SAM.BusinessTier.Payload.Order;
using SAM.BusinessTier.Payload.Rank;
using SAM.BusinessTier.Payload.Task;
using SAM.BusinessTier.Payload.Wards;
using SAM.BusinessTier.Payload.Warranty;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace SAM.BusinessTier.Services.Implements
{
    public class TaskService : BaseService<TaskService>, ITaskService
    {
        private readonly INotificationService _notificationService;

        public TaskService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<TaskService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, INotificationService notificationService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _notificationService = notificationService;
        }

        public async Task<Guid> CreateNewTask(CreateNewTaskRequest request)
        {
            DateTime currentTime = TimeUtils.GetCurrentSEATime();
            Guid? addressId = null;
            var tasksForToday = await _unitOfWork.GetRepository<TaskManager>().GetListAsync(
                predicate: t => t.AccountId == request.AccountId && t.ExcutionDate.HasValue && t.ExcutionDate.Value.Date == request.ExcutionDate.Value.Date);
            int taskCountForToday = tasksForToday.Count();

            // Lấy danh sách tất cả task của nhân viên với trạng thái Process
            var processTasks = await _unitOfWork.GetRepository<TaskManager>().GetListAsync(
                predicate: t => t.AccountId == request.AccountId && t.Status == TaskManagerStatus.Process.GetDescriptionFromEnum());
            int processTaskCount = processTasks.Count();


             //Kiểm tra số lượng task trong ngày của nhân viên
            if (taskCountForToday >= 3)
            {
                throw new BadHttpRequestException(MessageConstant.TaskManager.FullTaskMessage);
            }

            if (request.WarrantyDetailId.HasValue)
            {
                var warrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
                    predicate: wd => wd.Id == request.WarrantyDetailId.Value);

                if (warrantyDetail == null)
                {
                    throw new BadHttpRequestException(MessageConstant.WarrantyDetail.WarrantyDetailNotFoundMessage);
                }

                addressId = warrantyDetail.AddressId;
                warrantyDetail.AccountId = request.AccountId;
                warrantyDetail.Status = WarrantyDetailStatus.Process.GetDescriptionFromEnum();
                warrantyDetail.StartDate = request.ExcutionDate;
                _unitOfWork.GetRepository<WarrantyDetail>().UpdateAsync(warrantyDetail);
            }
            else if (request.OrderId.HasValue)
            {
                var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                    predicate: o => o.Id == request.OrderId.Value);

                if (order == null)
                {
                    throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);
                }

                addressId = order.AddressId;
                order.Status = OrderStatus.Delivery.GetDescriptionFromEnum();
                order.CompletedDate = request.ExcutionDate;
                _unitOfWork.GetRepository<Order>().UpdateAsync(order);
            }
            else
            {
                throw new BadHttpRequestException(MessageConstant.TaskManager.TypeTaskMessage);
            }

            TaskManager newTask = new()
            {
                Id = Guid.NewGuid(),
                Type = request.Type,
                Status = TaskManagerStatus.Process.GetDescriptionFromEnum(),
                CreateDate = currentTime,
                ExcutionDate = request.ExcutionDate,
                AccountId = request.AccountId,
                WarrantyDetailId = request.WarrantyDetailId,
                OrderId = request.OrderId,
                AddressId = addressId
            };
            var title = "Có nhiệm vụ mới";
            var body = $"Nhiệm vụ của bạn là {request.Type}";
            await _notificationService.SendPushNotificationAsync(title, body,request.AccountId);
            var newNotifications = new Notification
            {
                Id = Guid.NewGuid(),
                Title = title,
                Message = body,
                AccountId = request.AccountId,
                CreatedDate = currentTime
            };
            if (!string.IsNullOrWhiteSpace(request.Note))
            {
                var warrantyNote = new WarrantyNote
                {
                    Id = Guid.NewGuid(),
                    Description = request.Note,
                    CreateDate = currentTime,
                    WarrantyDetailId = request.WarrantyDetailId,
                };

                await _unitOfWork.GetRepository<WarrantyNote>().InsertAsync(warrantyNote);
            }
            await _unitOfWork.GetRepository<Notification>().InsertAsync(newNotifications);
            await _unitOfWork.GetRepository<TaskManager>().InsertAsync(newTask);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new BadHttpRequestException(MessageConstant.TaskManager.CreateNewTaskFailedMessage);
            }

            return newTask.Id;
        }



        public async Task<GetTaskResponse> GetTaskById(Guid id)
        {
            var task = await _unitOfWork.GetRepository<TaskManager>()
                .SingleOrDefaultAsync(
                    predicate: x => x.Id.Equals(id),
                    selector: task => new GetTaskResponse
                    {
                        Id = task.Id,
                        Type = EnumUtil.ParseEnum<TaskType>(task.Type),
                        CreateDate = task.CreateDate,
                        Status = EnumUtil.ParseEnum<TaskManagerStatus>(task.Status),
                        CompletedDate = task.CompletedDate,
                        ExcutionDate = task.ExcutionDate,
                        WarrantyDetail = task.WarrantyDetail == null ? null : new WarrantyDetailResponse
                        {
                            Id = task.WarrantyDetail.Id,
                            Status = EnumUtil.ParseEnum<WarrantyDetailStatus>(task.WarrantyDetail.Status),
                            CreateDate = task.WarrantyDetail.CreateDate,
                            StartDate = task.WarrantyDetail.StartDate,
                            NextMaintenanceDate = task.WarrantyDetail.NextMaintenanceDate,
                            Description = task.WarrantyDetail.Description,
                            Comments = task.WarrantyDetail.Comments,
                            WarrantyId = task.WarrantyDetail.WarrantyId,
                            AccountId = task.WarrantyDetail.AccountId
                        },
                        Order = new OrderResponse
                        {
                            Id = task.Order.Id,
                            InvoiceCode = task.Order.InvoiceCode,
                            FinalAmount = task.Order.FinalAmount
                        },
                        Staff = task.Account == null ? null : new AccountResponse
                        {
                            Id = task.Account.Id,
                            FullName = task.Account.FullName,
                            Role = EnumUtil.ParseEnum<RoleEnum>(task.Account.Role),
                        },
                        Address = task.Address == null ? null : new GetAddressResponse
                        {
                            Id = task.Address.Id,
                            Name = task.Address.Name,
                            Status = EnumUtil.ParseEnum<AddressStatus>(task.Address.Status),
                            NamePersonal = task.Address.NamePersonal,
                            PhoneNumber = task.Address.PhoneNumber,
                            Note = task.Address.Note,
                            City = task.Address.City == null ? null : new CityResponse
                            {
                                Id = task.Address.City.Id,
                                UnitId = task.Address.City.UnitId,
                                Name = task.Address.City.Name
                            },
                            District = task.Address.District == null ? null : new DistrictResponse
                            {
                                Id = task.Address.District.Id,
                                UnitId = task.Address.District.UnitId,
                                Name = task.Address.District.Name
                            },
                            Ward = task.Address.Ward == null ? null : new WardResponse
                            {
                                Id = task.Address.Ward.Id,
                                UnitId = task.Address.Ward.UnitId,
                                Name = task.Address.Ward.Name
                            },
                            Account = task.Address.Account == null ? null : new AccountResponse
                            {
                                Id = task.Address.Account.Id,
                                FullName = task.Address.Account.FullName,
                                Role = EnumUtil.ParseEnum<RoleEnum>(task.Address.Account.Role),

                            }
                        }
                    },
                    orderBy: null, 
                    include: x => x.Include(x => x.WarrantyDetail)
                                   .Include(x => x.Order)
                                       .ThenInclude(o => o.Account)
                                   .Include(x => x.Address)
                                       .ThenInclude(a => a.City)
                                   .Include(x => x.Address)
                                       .ThenInclude(a => a.District)
                                   .Include(x => x.Address)
                                       .ThenInclude(a => a.Ward)
                                   .Include(x => x.Address)
                                       .ThenInclude(a => a.Account)
                );

            return task;
        }



        public async Task<ICollection<GetTaskResponse>> GetTaskList(TaskFilter filter)
        {
            var taskList = await _unitOfWork.GetRepository<TaskManager>()
                .GetListAsync(
                    selector: task => new GetTaskResponse
                    {
                        Id = task.Id,
                        Type = EnumUtil.ParseEnum<TaskType>(task.Type),
                        CreateDate = task.CreateDate,
                        Status = EnumUtil.ParseEnum<TaskManagerStatus>(task.Status),
                        CompletedDate = task.CompletedDate,
                        ExcutionDate = task.ExcutionDate,
                        WarrantyDetail = task.WarrantyDetail == null ? null : new WarrantyDetailResponse
                        {
                            Id = task.WarrantyDetail.Id,
                            Status = EnumUtil.ParseEnum<WarrantyDetailStatus>(task.WarrantyDetail.Status),
                            CreateDate = task.WarrantyDetail.CreateDate,
                            StartDate = task.WarrantyDetail.StartDate,
                            NextMaintenanceDate = task.WarrantyDetail.NextMaintenanceDate,
                            Description = task.WarrantyDetail.Description,
                            Comments = task.WarrantyDetail.Comments,
                            WarrantyId = task.WarrantyDetail.WarrantyId,
                            AccountId = task.WarrantyDetail.AccountId
                        },
                        Order = new OrderResponse
                        {
                            Id = task.Order.Id,
                            InvoiceCode = task.Order.InvoiceCode,
                            FinalAmount = task.Order.FinalAmount
                        },
                        Staff = task.Account == null ? null : new AccountResponse
                        {
                            Id = task.Account.Id,
                            FullName = task.Account.FullName,
                            Role = EnumUtil.ParseEnum<RoleEnum>(task.Account.Role),
                        },
                        Address = task.Address == null ? null : new GetAddressResponse
                        {
                            Id = task.Address.Id,
                            Name = task.Address.Name,
                            Status = EnumUtil.ParseEnum<AddressStatus>(task.Address.Status),
                            NamePersonal = task.Address.NamePersonal,
                            PhoneNumber = task.Address.PhoneNumber,
                            Note = task.Address.Note,
                            City = task.Address.City == null ? null : new CityResponse
                            {
                                Id = task.Address.City.Id,
                                UnitId = task.Address.City.UnitId,
                                Name = task.Address.City.Name
                            },
                            District = task.Address.District == null ? null : new DistrictResponse
                            {
                                Id = task.Address.District.Id,
                                UnitId = task.Address.District.UnitId,
                                Name = task.Address.District.Name
                            },
                            Ward = task.Address.Ward == null ? null : new WardResponse
                            {
                                Id = task.Address.Ward.Id,
                                UnitId = task.Address.Ward.UnitId,
                                Name = task.Address.Ward.Name
                            },
                            Account = task.Address.Account == null ? null : new AccountResponse
                            {
                                Id = task.Address.Account.Id,
                                FullName = task.Address.Account.FullName,
                                Role = EnumUtil.ParseEnum<RoleEnum>(task.Address.Account.Role),

                            }
                        }
                    },
                    filter: filter,
                    orderBy: null, 
                    include: x => x.Include(x => x.WarrantyDetail)
                                   .Include(x => x.Order)
                                       .ThenInclude(o => o.Account)
                                   .Include(x => x.Address)
                                       .ThenInclude(a => a.City)
                                   .Include(x => x.Address)
                                       .ThenInclude(a => a.District)
                                   .Include(x => x.Address)
                                       .ThenInclude(a => a.Ward)
                                   .Include(x => x.Address)
                                       .ThenInclude(a => a.Account)
                );

            return taskList;
        }



        public async Task<bool> UpdateTask(Guid id, UpdateTaskRequest updateTaskRequest)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException(MessageConstant.TaskManager.EmptyTaskIdMessage);

            TaskManager task = await _unitOfWork.GetRepository<TaskManager>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.TaskManager.TaskNameExisted);

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateTaskRequest.AccountId))
                ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);

            Address address = await _unitOfWork.GetRepository<Address>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateTaskRequest.AddressId))
                ?? throw new BadHttpRequestException(MessageConstant.Address.NotFoundFailedMessage);

            task.AccountId = updateTaskRequest.AccountId;
            task.AddressId = updateTaskRequest.AddressId;
            updateTaskRequest.ExcutionDate = updateTaskRequest.ExcutionDate.HasValue ? task.ExcutionDate : updateTaskRequest.ExcutionDate;

            _unitOfWork.GetRepository<TaskManager>().UpdateAsync(task);

            if (task.WarrantyDetailId.HasValue)
            {
                var warrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
                    predicate: wd => wd.Id == task.WarrantyDetailId.Value);

                if (warrantyDetail == null)
                {
                    throw new BadHttpRequestException(MessageConstant.WarrantyDetail.WarrantyDetailNotFoundMessage);
                }

                warrantyDetail.AccountId = updateTaskRequest.AccountId;
                warrantyDetail.AddressId = updateTaskRequest.AddressId;
                _unitOfWork.GetRepository<WarrantyDetail>().UpdateAsync(warrantyDetail);
            }

            if (task.OrderId.HasValue)
            {
                var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                    predicate: o => o.Id == task.OrderId.Value);

                if (order == null)
                {
                    throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);
                }
                order.AddressId = updateTaskRequest.AddressId;
                _unitOfWork.GetRepository<Order>().UpdateAsync(order);
            }

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

    }
}
