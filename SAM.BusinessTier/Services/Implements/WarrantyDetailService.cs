using AutoMapper;
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
        public WarrantyDetailService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<WarrantyDetailService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
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
                        Note = detail.WarrantyNotes.Select(note => new WarrantyDetailNoteResponse
                        {
                            Id = note.Id,
                            Description = note.Description,
                            CreateDate = note.CreateDate,
                            Image = note.Image
                        }).ToList(),
                    },
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.StartDate),
                    include: x => x.Include(x => x.Account)
                                   .Include(x => x.ComponentChanges)
                                   .Include(x => x.WarrantyNotes)
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
                        Note = detail.WarrantyNotes.Select(note => new WarrantyDetailNoteResponse
                        {
                            Id = note.Id,
                            Description = note.Description,
                            CreateDate = note.CreateDate,
                            Image = note.Image
                        }).ToList(),

                    },
                    predicate: detail => detail.Id == id,
                    include: x => x.Include(detail => detail.Account)
                                   .Include(detail => detail.Warranty.Inventory)
                                   .Include(detail => detail.ComponentChanges)
                                       .ThenInclude(part => part.MachineComponent)
                                    .Include(x => x.WarrantyNotes)
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

        //public async Task<bool> UpdateWarrantyDetail(Guid id, UpdateWarrantyDetailRequest updateDetailRequest)
        //{
        //    if (id == Guid.Empty)
        //        throw new BadHttpRequestException(MessageConstant.WarrantyDetail.EmptyWarrantyDetailIdMessage);

        //    DateTime currentTime = TimeUtils.GetCurrentSEATime();

        //    WarrantyDetail warrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
        //        predicate: x => x.Id.Equals(id))
        //    ?? throw new BadHttpRequestException(MessageConstant.WarrantyDetail.WarrantyDetailNotFoundMessage);

        //    // Update warranty detail properties
        //    warrantyDetail.Description = !string.IsNullOrEmpty(updateDetailRequest.Description) ? updateDetailRequest.Description : warrantyDetail.Description;
        //    warrantyDetail.Comments = !string.IsNullOrEmpty(updateDetailRequest.Comments) ? updateDetailRequest.Comments : warrantyDetail.Comments;
        //    warrantyDetail.NextMaintenanceDate = updateDetailRequest.NextMaintenanceDate.HasValue ? updateDetailRequest.NextMaintenanceDate.Value : warrantyDetail.NextMaintenanceDate;

        //    if (!updateDetailRequest.Status.HasValue)
        //    {
        //        throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
        //    }
        //    else
        //    {
        //        warrantyDetail.Status = updateDetailRequest.Status.GetDescriptionFromEnum();
        //    }

        //    // Update Account if provided
        //    if (updateDetailRequest.AccountId.HasValue)
        //    {
        //        Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
        //            predicate: x => x.Id.Equals(updateDetailRequest.AccountId.Value))
        //        ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);

        //        warrantyDetail.Account = account;
        //    }

        //    // Update or add component changes
        //    if (updateDetailRequest.ComponentId != null && updateDetailRequest.ComponentId.Count > 0)
        //    {
        //        foreach (var componentId in updateDetailRequest.ComponentId)
        //        {
        //            var component = await _unitOfWork.GetRepository<MachineComponent>().SingleOrDefaultAsync(
        //                predicate: x => x.Id.Equals(componentId))
        //            ?? throw new BadHttpRequestException(MessageConstant.MachineryComponents.MachineryComponentsNotFoundMessage);

        //            var componentChange = new ComponentChange
        //            {
        //                Id = Guid.NewGuid(),
        //                WarrantyDetailId = warrantyDetail.Id,
        //                MachineComponentId = component.Id,
        //                CreateDate = currentTime,
        //                Image = updateDetailRequest.Image,
        //            };
        //            await _unitOfWork.GetRepository<ComponentChange>().InsertAsync(componentChange);
        //        }
        //    }

        //    // Handle status "Completed" scenario
        //    if (updateDetailRequest.Status.GetDescriptionFromEnum() == "Completed")
        //    {
        //        warrantyDetail.CompletionDate = currentTime;

        //        var taskManager = await _unitOfWork.GetRepository<TaskManager>().SingleOrDefaultAsync(
        //            predicate: t => t.WarrantyDetailId == warrantyDetail.Id);

        //        if (taskManager != null)
        //        {
        //            taskManager.Status = TaskManagerStatus.Completed.GetDescriptionFromEnum();
        //            _unitOfWork.GetRepository<TaskManager>().UpdateAsync(taskManager);
        //        }

        //        var nextWarrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
        //            predicate: wd => wd.WarrantyId == warrantyDetail.WarrantyId
        //                             && wd.StartDate > warrantyDetail.StartDate
        //                             && wd.Status != WarrantyDetailStatus.Completed.GetDescriptionFromEnum(),
        //            orderBy: q => q.OrderBy(wd => wd.StartDate));

        //        Warranty warranty = await _unitOfWork.GetRepository<Warranty>().SingleOrDefaultAsync(
        //            predicate: w => w.Id == warrantyDetail.WarrantyId)
        //        ?? throw new BadHttpRequestException(MessageConstant.Warranty.WarrantyNotFoundMessage);

        //        if (nextWarrantyDetail != null)
        //        {
        //            warranty.NextMaintenanceDate = nextWarrantyDetail.StartDate;
        //        }
        //        else
        //        {
        //            warranty.NextMaintenanceDate = null;
        //            warranty.CompletionDate = DateTime.Now;
        //        }

        //        _unitOfWork.GetRepository<Warranty>().UpdateAsync(warranty);
        //    }

        //    _unitOfWork.GetRepository<WarrantyDetail>().UpdateAsync(warrantyDetail);
        //    bool isSuccess = await _unitOfWork.CommitAsync() > 0;
        //    return isSuccess;
        //}

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

            // Prepare to create a note
            string noteDescription = string.Empty;

            // Update or add component changes and create a note for them
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
                    };
                    await _unitOfWork.GetRepository<ComponentChange>().InsertAsync(componentChange);

                    // Append component info to the note description
                    noteDescription += $"Bộ Phận: {component.Name}, Giá: {component.SellingPrice}\n";
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
                    warranty.Status = WarrantyStatus.Completed.GetDescriptionFromEnum();
                }

                _unitOfWork.GetRepository<Warranty>().UpdateAsync(warranty);

                // Append completion info to the note description
                noteDescription += "Bảo trì hoàn tất.\n";
            }

            // Handle status "Canceled" scenario with note entry
            if (updateDetailRequest.Status.GetDescriptionFromEnum() == "Canceled")
            {
                if (string.IsNullOrEmpty(updateDetailRequest.Description))
                {
                    throw new BadHttpRequestException("Nội dung khi hủy");
                }

                // Create a note with the cancellation reason
                noteDescription += $"Hủy yêu cầu bảo hành vì: {updateDetailRequest.Description}\n";
            }

            // Create the note if there's any description to add
            if (!string.IsNullOrEmpty(noteDescription))
            {
                var warrantyNote = new WarrantyNote
                {
                    Id = Guid.NewGuid(),
                    Description = noteDescription.Trim(),
                    CreateDate = currentTime,
                    WarrantyDetailId = warrantyDetail.Id,
                    Image = updateDetailRequest.Image
                };

                await _unitOfWork.GetRepository<WarrantyNote>().InsertAsync(warrantyNote);
            }

            _unitOfWork.GetRepository<WarrantyDetail>().UpdateAsync(warrantyDetail);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }


        public async Task<Guid> CreateOrderForReplacedComponents(CreateNewOrderForWarrantyComponentRequest createNewOrderForWarrantyComponent)
        {
            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            WarrantyDetail warrantyDetail = await _unitOfWork.GetRepository<WarrantyDetail>().SingleOrDefaultAsync(
                predicate: wd => wd.Id == createNewOrderForWarrantyComponent.WarrantyId,
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
                AccountId = createNewOrderForWarrantyComponent.AccountId,
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
