﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Enums.Other;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.Order;
using SAM.BusinessTier.Payload.Task;
using SAM.BusinessTier.Payload.Wards;
using SAM.BusinessTier.Payload.Warranty;
using SAM.BusinessTier.Payload.WarrantyDetail;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Implements
{
    public class WarrantyDetailService : BaseService<WarrantyDetailService>, IWarrantyDetailService
    {
        public WarrantyDetailService(IUnitOfWork<SamContext> unitOfWork, ILogger<WarrantyDetailService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewWarrantyDetail(CreateNewWarrantyDetailRequest createNewWarrantyDetailRequest)
        {
            throw new NotImplementedException();
        }
        public async Task<ICollection<GetWarrantyDetailResponse>> GetWarrantyDetailList(WarrantyDetailFilter filter)
        {
            var warrantyDetails = await _unitOfWork.GetRepository<WarrantyDetail>()
                .GetListAsync(
                    selector: detail => new GetWarrantyDetailResponse
                    {
                        Id = detail.Id,
                        Type = detail.Type != null ? EnumUtil.ParseEnum<WarrantyDetailType>(detail.Type) : null,
                        Status = detail.Status != null ? EnumUtil.ParseEnum<WarrantyDetailStatus>(detail.Status) : null,
                        CreateDate = detail.CreateDate,
                        StartDate = detail.StartDate,
                        CompletionDate = detail.CompletionDate,
                        Description = detail.Description,
                        Comments = detail.Comments,
                        NextMaintenanceDate = detail.NextMaintenanceDate,
                        WarrantyId = detail.WarrantyId,
                        Staff = detail.Account != null ? new OrderUserResponse
                        {
                            Id = detail.Account.Id,
                            FullName = detail.Account.FullName,
                            Role = EnumUtil.ParseEnum<RoleEnum>(detail.Account.Role)
                        } : null,
                        ComponentChange = detail.ComponentChanges.Select(part => new ComponentChangeResponse
                        {
                            Image = part.Image,
                            CreateDate = part.CreateDate,
                            Component = new ComponentResponse
                            {
                                Id = part.MachineComponent.Id,
                                Name = part.MachineComponent.Name,
                                Description = part.MachineComponent.Description,
                                Status = EnumUtil.ParseEnum<ComponentStatus>(part.MachineComponent.Status),
                                StockPrice = part.MachineComponent.StockPrice,
                                SellingPrice = part.MachineComponent.SellingPrice
                            }
                        }).ToList(),
                    },
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.StartDate),
                    include: x => x.Include(x => x.Account)
                                   .Include(x => x.ComponentChanges)
                ) ?? throw new BadHttpRequestException(MessageConstant.WarrantyDetail.WarrantyDetailNotFoundMessage);


            var warrantyDetailIds = warrantyDetails.Select(wd => wd.Id).ToList();

            return warrantyDetails;
        }



        public async Task<GetWarrantyDetailResponse> GetWarrantyDetailById(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException("Invalid warranty detail ID.");

            var warrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>()
                .SingleOrDefaultAsync(
                    selector: detail => new GetWarrantyDetailResponse
                    {
                        Id = detail.Id,
                        Type = detail.Type != null ? EnumUtil.ParseEnum<WarrantyDetailType>(detail.Type) : null,
                        CreateDate = detail.CreateDate,
                        StartDate = detail.StartDate,
                        CompletionDate = detail.CompletionDate,
                        Status = detail.Status != null ? EnumUtil.ParseEnum<WarrantyDetailStatus>(detail.Status) : null,
                        Description = detail.Description,
                        Comments = detail.Comments,
                        WarrantyId = detail.WarrantyId,
                        NextMaintenanceDate = detail.NextMaintenanceDate,
                        Staff = detail.Account != null ? new OrderUserResponse
                        {
                            Id = detail.Account.Id,
                            FullName = detail.Account.FullName,
                            Role = EnumUtil.ParseEnum<RoleEnum>(detail.Account.Role)
                        } : null,
                        ComponentChange = detail.ComponentChanges.Select(part => new ComponentChangeResponse
                        {
                            Image = part.Image,
                            CreateDate = part.CreateDate,
                            Component = new ComponentResponse
                            {
                                Id = part.MachineComponent.Id,
                                Name = part.MachineComponent.Name,
                                Description = part.MachineComponent.Description,
                                Status = EnumUtil.ParseEnum<ComponentStatus>(part.MachineComponent.Status),
                                StockPrice = part.MachineComponent.StockPrice,
                                SellingPrice = part.MachineComponent.SellingPrice
                            }
                        }).ToList(),
                    },
                    predicate: detail => detail.Id == id,
                    include: x => x.Include(detail => detail.Account)
                                   .Include(detail => detail.Warranty.Inventory)
                                   .Include(detail => detail.ComponentChanges)
                                       .ThenInclude(part => part.MachineComponent)
                );

            if (warrantyDetail == null)
            {
                throw new BadHttpRequestException($"Warranty detail with ID {id} not found.");
            }

            return warrantyDetail;
        }




        public Task<bool> RemoveWarrantyDetailStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWarrantyDetail(Guid id, UpdateWarrantyDetailRequest updateDetailRequest)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException(MessageConstant.WarrantyDetail.EmptyWarrantyDetailIdMessage);

            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            WarrantyDetail warrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.WarrantyDetail.WarrantyDetailNotFoundMessage);

            // Update warranty detail properties
            warrantyDetail.Description = !string.IsNullOrEmpty(updateDetailRequest.Description) ? updateDetailRequest.Description : warrantyDetail.Description;
            warrantyDetail.Comments = !string.IsNullOrEmpty(updateDetailRequest.Comments) ? updateDetailRequest.Comments : warrantyDetail.Comments;
            warrantyDetail.NextMaintenanceDate = updateDetailRequest.NextMaintenanceDate.HasValue ? updateDetailRequest.NextMaintenanceDate.Value : warrantyDetail.NextMaintenanceDate;

            if (!updateDetailRequest.Status.HasValue)
            {
                throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
            }
            else
            {
                warrantyDetail.Status = updateDetailRequest.Status.GetDescriptionFromEnum();
            }

            // Update Account if provided
            if (updateDetailRequest.AccountId.HasValue)
            {
                Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                    predicate: x => x.Id.Equals(updateDetailRequest.AccountId.Value))
                ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);

                warrantyDetail.Account = account;
            }

            // Update or add component changes
            if (updateDetailRequest.ComponentId != null && updateDetailRequest.ComponentId.Count > 0)
            {
                foreach (var componentId in updateDetailRequest.ComponentId)
                {
                    var component = await _unitOfWork.GetRepository<MachineComponent>().SingleOrDefaultAsync(
                        predicate: x => x.Id.Equals(componentId))
                    ?? throw new BadHttpRequestException(MessageConstant.MachineryComponents.MachineryComponentsNotFoundMessage);

                    var componentChange = new ComponentChange
                    {
                        Id = Guid.NewGuid(),
                        WarrantyDetailId = warrantyDetail.Id,
                        MachineComponentId = component.Id,
                        CreateDate = currentTime,
                        Image = updateDetailRequest.Image,
                    };
                    await _unitOfWork.GetRepository<ComponentChange>().InsertAsync(componentChange);
                }
            }

            // Handle status "Completed" scenario
            if (updateDetailRequest.Status.GetDescriptionFromEnum() == "Completed")
            {
                warrantyDetail.CompletionDate = currentTime;

                var taskManager = await _unitOfWork.GetRepository<TaskManager>().SingleOrDefaultAsync(
                    predicate: t => t.WarrantyDetailId == warrantyDetail.Id);

                if (taskManager != null)
                {
                    taskManager.Status = TaskManagerStatus.Completed.GetDescriptionFromEnum();
                    _unitOfWork.GetRepository<TaskManager>().UpdateAsync(taskManager);
                }

                var nextWarrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
                    predicate: wd => wd.WarrantyId == warrantyDetail.WarrantyId
                                     && wd.StartDate > warrantyDetail.StartDate
                                     && wd.Status != WarrantyDetailStatus.Completed.GetDescriptionFromEnum(),
                    orderBy: q => q.OrderBy(wd => wd.StartDate));

                Warranty warranty = await _unitOfWork.GetRepository<Warranty>().SingleOrDefaultAsync(
                    predicate: w => w.Id == warrantyDetail.WarrantyId)
                ?? throw new BadHttpRequestException(MessageConstant.Warranty.WarrantyNotFoundMessage);

                if (nextWarrantyDetail != null)
                {
                    warranty.NextMaintenanceDate = nextWarrantyDetail.StartDate;
                }
                else
                {
                    warranty.NextMaintenanceDate = null;
                    warranty.CompletionDate = DateTime.Now;
                }

                _unitOfWork.GetRepository<Warranty>().UpdateAsync(warranty);
            }

            _unitOfWork.GetRepository<WarrantyDetail>().UpdateAsync(warrantyDetail);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

        public async Task<Guid> CreateOrderForReplacedComponents(Guid warrantyDetailId, Guid accountId)
        {
            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            WarrantyDetail warrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
                predicate: wd => wd.Id == warrantyDetailId,
                include: wd => wd.Include(wd => wd.ComponentChanges)
                                 .ThenInclude(cc => cc.MachineComponent))
                ?? throw new BadHttpRequestException(MessageConstant.WarrantyDetail.WarrantyDetailNotFoundMessage);

            if (warrantyDetail.ComponentChanges == null || !warrantyDetail.ComponentChanges.Any())
            {
                throw new BadHttpRequestException("Không có bộ phận trong hệ thống để tạo hóa đơn");
            }

            Order newOrder = new Order
            {
                Id = Guid.NewGuid(),
                InvoiceCode = TimeUtils.GetTimestamp(currentTime),
                CreateDate = currentTime,
                CompletedDate = null,
                TotalAmount = 0,
                Status = OrderStatus.UnPaid.GetDescriptionFromEnum(),
                Type = OrderType.Warranty.GetDescriptionFromEnum(),
                AccountId = accountId,
                AddressId = warrantyDetail.AddressId,
                Description = "Thanh toán cho bộ phận sửa chữa",
            };

            double totalAmount = 0;

            foreach (var componentChange in warrantyDetail.ComponentChanges)
            {
                if (componentChange.MachineComponent == null)
                {
                    throw new BadHttpRequestException(MessageConstant.MachineryComponents.MachineryComponentsNotFoundMessage);
                }

                OrderDetail orderDetail = new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = newOrder.Id,
                    MachineComponentId = componentChange.MachineComponent.Id,
                    Quantity = 1,
                    SellingPrice = componentChange.MachineComponent.SellingPrice,
                    TotalAmount = componentChange.MachineComponent.SellingPrice,
                    CreateDate = DateTime.UtcNow
                };

                totalAmount += orderDetail.TotalAmount ?? 0;
                await _unitOfWork.GetRepository<OrderDetail>().InsertAsync(orderDetail);
            }

            newOrder.FinalAmount = totalAmount;
            newOrder.TotalAmount = totalAmount;

            await _unitOfWork.GetRepository<Order>().InsertAsync(newOrder);
            await _unitOfWork.CommitAsync();

            return newOrder.Id;
        }





    }
}
