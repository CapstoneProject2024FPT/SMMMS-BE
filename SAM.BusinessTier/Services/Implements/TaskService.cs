using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Address;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Payload.News;
using SAM.BusinessTier.Payload.Order;
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
        public TaskService(IUnitOfWork<SamContext> unitOfWork, ILogger<TaskService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewTask(CreateNewTaskRequest request)
        {
            var currentUser = GetUsernameFromJwt();
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser));
            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            Guid? addressId = null;

            if (request.WarrantyDetailId.HasValue)
            {
                var warrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
                    predicate: wd => wd.Id == request.WarrantyDetailId.Value);

                if (warrantyDetail == null)
                {
                    throw new BadHttpRequestException("WarrantyDetail not found.");
                }

                addressId = warrantyDetail.AddressId;
            }
            else if (request.OrderId.HasValue)
            {
                var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                    predicate: o => o.Id == request.OrderId.Value);

                if (order == null)
                {
                    throw new BadHttpRequestException("Order not found.");
                }

                addressId = order.AddressId;
                order.Status = OrderStatus.Delivery.GetDescriptionFromEnum(); // Assuming OrderStatus.Delivery is defined

                // Update the order asynchronously
                _unitOfWork.GetRepository<Order>().UpdateAsync(order);
            }
            else
            {
                throw new BadHttpRequestException("Either WarrantyDetailId or OrderId must be provided.");
            }

            TaskManager newTask = new()
            {
                Id = Guid.NewGuid(),
                Type = request.Type,
                Status = TaskManagerStatus.Process.GetDescriptionFromEnum(),
                CreateDate = currentTime,
                AccountId = request.AccountId,
                WarrantyDetailId = request.WarrantyDetailId,
                OrderId = request.OrderId,
                AddressId = addressId
            };

            await _unitOfWork.GetRepository<TaskManager>().InsertAsync(newTask);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessful)
            {
                throw new BadHttpRequestException("Failed to create a new task.");
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
                        WarrantyDetail = task.WarrantyDetail == null ? null : new WarrantyDetailResponse
                        {
                            Id = task.WarrantyDetail.Id,
                            Status = EnumUtil.ParseEnum<WarrantyDetailStatus>(task.WarrantyDetail.Status),
                            CreateDate = task.WarrantyDetail.CreateDate,
                            StartDate = task.WarrantyDetail.StartDate,
                            Description = task.WarrantyDetail.Description,
                            Comments = task.WarrantyDetail.Comments,
                            WarrantyId = task.WarrantyDetail.WarrantyId,
                            AccountId = task.WarrantyDetail.AccountId
                        },
                        Order = new OrderResponse
                        {
                            Id = task.Order.Id,
                            InvoiceCode = task.Order.InvoiceCode,
                            Note = task.Order.Note,
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
                    orderBy: null, // Replace with your desired order by expression if needed
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
                        WarrantyDetail = task.WarrantyDetail == null ? null : new WarrantyDetailResponse
                        {
                            Id = task.WarrantyDetail.Id,
                            Status = EnumUtil.ParseEnum<WarrantyDetailStatus>(task.WarrantyDetail.Status),
                            CreateDate = task.WarrantyDetail.CreateDate,
                            StartDate = task.WarrantyDetail.StartDate,
                            Description = task.WarrantyDetail.Description,
                            Comments = task.WarrantyDetail.Comments,
                            WarrantyId = task.WarrantyDetail.WarrantyId,
                            AccountId = task.WarrantyDetail.AccountId
                        },
                        Order = new OrderResponse
                        {
                            Id = task.Order.Id,
                            InvoiceCode = task.Order.InvoiceCode,
                            Note = task.Order.Note,
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
                    orderBy: null, // Replace with your desired order by expression if needed
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


        public Task<bool> RemoveTaskStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTask(Guid id, UpdateTaskRequest updateTaskRequest)
        {
            throw new NotImplementedException();
        }
    }
}
