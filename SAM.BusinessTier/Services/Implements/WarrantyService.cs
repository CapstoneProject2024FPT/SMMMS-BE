using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Extensions;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.News;
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
    public class WarrantyService : BaseService<WarrantyService>, IWarrantyService
    {
        public WarrantyService(IUnitOfWork<SamContext> unitOfWork, ILogger<WarrantyService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task<Guid> CreateNewWarranty(CreateNewWarrantyRequest createNewWarrantyRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<GetWarrantyInforResponse>> GetWarrantyList(WarrantyFilter filter)
        {
            var warrantyList = await _unitOfWork.GetRepository<Warranty>()
                .GetListAsync(
                    selector: warranty => new GetWarrantyInforResponse
                    {
                        Id = warranty.Id,
                        Type = EnumUtil.ParseEnum<WarrantyType>(warranty.Type),
                        CreateDate = warranty.CreateDate,
                        StartDate = warranty.StartDate,
                        CompletionDate = warranty.CompletionDate,
                        Status = EnumUtil.ParseEnum<WarrantyStatus>(warranty.Status),
                        Description = warranty.Description,
                        Comments = warranty.Comments,
                        NextMaintenanceDate = warranty.NextMaintenanceDate,
                        Priority = warranty.Priority,
                        WarrantyDetai = warranty.WarrantyDetails.CountWarrantyDetailEachStatus(),
                        Inventory = new InventoryResponse
                        {
                            Id = warranty.Inventory.Id,
                            SerialNumber = warranty.Inventory.SerialNumber,
                            Type = EnumUtil.ParseEnum<InventoryType>(warranty.Inventory.Type),
                            Machinery = new GetMachinerySpecificationsRespone
                            {
                                Id = warranty.Inventory.Machinery.Id,
                                Name = warranty.Inventory.Machinery.Name,
                                Brand = new BrandResponse()
                                {
                                    Id = warranty.Inventory.Machinery.BrandId,
                                    Name = warranty.Inventory.Machinery.Brand.Name,
                                    Description = warranty.Inventory.Machinery.Brand.Description,
                                },
                                Model = warranty.Inventory.Machinery.Model,
                                Description = warranty.Inventory.Machinery.Description,
                                SellingPrice = warranty.Inventory.Machinery.SellingPrice,
                                Priority = warranty.Inventory.Machinery.Priority,
                                TimeWarranty = warranty.Inventory.Machinery.TimeWarranty,
                                Status = EnumUtil.ParseEnum<MachineryStatus>(warranty.Inventory.Machinery.Status),
                                CreateDate = warranty.Inventory.Machinery.CreateDate,
                                Origin = new OriginResponse()
                                {
                                    Id = warranty.Inventory.Machinery.OriginId,
                                    Name = warranty.Inventory.Machinery.Origin.Name,
                                    Description = warranty.Inventory.Machinery.Origin.Description,
                                },
                                Category = new CategoryResponse()
                                {
                                    Id = warranty.Inventory.Machinery.CategoryId,
                                    Name = warranty.Inventory.Machinery.Category.Name,
                                    Type = EnumUtil.ParseEnum<CategoryType>(warranty.Inventory.Machinery.Category.Type),
                                },
                                Image = warranty.Inventory.Machinery.ImagesAlls.Select(image => new MachineryImagesResponse
                                {
                                    Id = image.Id,
                                    ImageURL = image.ImageUrl,
                                    CreateDate = image.CreateDate
                                }).ToList(),
                                Specifications = warranty.Inventory.Machinery.Specifications.Select(spec => new SpecificationsResponse
                                {
                                    SpecificationId = spec.Id,
                                    MachineryId = spec.MachineryId,
                                    Name = spec.Name,
                                    Value = spec.Value
                                }).ToList(),
                                Quantity = warranty.Inventory.Machinery.Inventories.CountInventoryEachStatus()
                            }
                        },
                        // Add Account information
                        Customer = warranty.Inventory.OrderDetails
                            .Where(detail => detail.MachineryId == warranty.Inventory.Machinery.Id)
                            .Select(detail => new AccountResponse
                            {
                                Id = detail.Order.Account.Id,
                                FullName = detail.Order.Account.FullName,
                                Role = EnumUtil.ParseEnum<RoleEnum>(detail.Order.Account.Role),
                            }).FirstOrDefault()
                    },
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.CreateDate),
                    include: x => x.Include(x => x.WarrantyDetails)
                                   .Include(x => x.Inventory)
                                       .ThenInclude(inventory => inventory.Machinery)
                                           .ThenInclude(machinery => machinery.Brand)
                                   .Include(x => x.Inventory)
                                       .ThenInclude(inventory => inventory.Machinery)
                                           .ThenInclude(machinery => machinery.Origin)
                                   .Include(x => x.Inventory)
                                       .ThenInclude(inventory => inventory.Machinery)
                                           .ThenInclude(machinery => machinery.Category)
                                   .Include(x => x.Inventory)
                                       .ThenInclude(inventory => inventory.Machinery)
                                           .ThenInclude(machinery => machinery.ImagesAlls)
                                   .Include(x => x.Inventory)
                                       .ThenInclude(inventory => inventory.Machinery)
                                           .ThenInclude(machinery => machinery.Specifications)
                                   // Include OrderDetails and Account for retrieving Account info
                                   .Include(x => x.Inventory.OrderDetails)
                                       .ThenInclude(detail => detail.Order.Account)
                ) ?? throw new BadHttpRequestException(MessageConstant.Warranty.WarrantyNotFoundMessage);

            return warrantyList;
        }




        public async Task<GetDetailWarrantyInfor> GetWarrantyById(Guid id)
        {
            var warranty = await _unitOfWork.GetRepository<Warranty>()
                .SingleOrDefaultAsync(
                    selector: warranty => new GetDetailWarrantyInfor
                    {
                        Id = warranty.Id,
                        Type = EnumUtil.ParseEnum<WarrantyType>(warranty.Type),
                        CreateDate = warranty.CreateDate,
                        StartDate = warranty.StartDate,
                        CompletionDate = warranty.CompletionDate,
                        Status = EnumUtil.ParseEnum<WarrantyStatus>(warranty.Status),
                        Description = warranty.Description,
                        Comments = warranty.Comments,
                        NextMaintenanceDate = warranty.NextMaintenanceDate,
                        Priority = warranty.Priority,
                        Inventory = new InventoryResponse
                        {
                            Id = warranty.Inventory.Id,
                            SerialNumber = warranty.Inventory.SerialNumber,
                            Type = EnumUtil.ParseEnum<InventoryType>(warranty.Inventory.Type),
                            Machinery = new GetMachinerySpecificationsRespone
                            {
                                Id = warranty.Inventory.Machinery.Id,
                                Name = warranty.Inventory.Machinery.Name,
                                Brand = new BrandResponse()
                                {
                                    Id = warranty.Inventory.Machinery.BrandId,
                                    Name = warranty.Inventory.Machinery.Brand.Name,
                                    Description = warranty.Inventory.Machinery.Brand.Description,
                                },
                                Model = warranty.Inventory.Machinery.Model,
                                Description = warranty.Inventory.Machinery.Description,
                                SellingPrice = warranty.Inventory.Machinery.SellingPrice,
                                Priority = warranty.Inventory.Machinery.Priority,
                                TimeWarranty = warranty.Inventory.Machinery.TimeWarranty,
                                Status = EnumUtil.ParseEnum<MachineryStatus>(warranty.Inventory.Machinery.Status),
                                CreateDate = warranty.Inventory.Machinery.CreateDate,
                                Origin = new OriginResponse()
                                {
                                    Id = warranty.Inventory.Machinery.OriginId,
                                    Name = warranty.Inventory.Machinery.Origin.Name,
                                    Description = warranty.Inventory.Machinery.Origin.Description,
                                },
                                Category = new CategoryResponse()
                                {
                                    Id = warranty.Inventory.Machinery.CategoryId,
                                    Name = warranty.Inventory.Machinery.Category.Name,
                                    Type = EnumUtil.ParseEnum<CategoryType>(warranty.Inventory.Machinery.Category.Type),
                                },
                                Image = warranty.Inventory.Machinery.ImagesAlls.Select(image => new MachineryImagesResponse
                                {
                                    Id = image.Id,
                                    ImageURL = image.ImageUrl,
                                    CreateDate = image.CreateDate
                                }).ToList(),
                                Specifications = warranty.Inventory.Machinery.Specifications.Select(spec => new SpecificationsResponse
                                {
                                    SpecificationId = spec.Id,
                                    MachineryId = spec.MachineryId,
                                    Name = spec.Name,
                                    Value = spec.Value
                                }).ToList(),
                                Quantity = warranty.Inventory.Machinery.Inventories.CountInventoryEachStatus()
                            }
                        },
                        WarrantyDetail = warranty.WarrantyDetails
                        .OrderBy(detail => detail.StartDate)
                        .Select(detail => new WarrantyDetailResponse
                        {
                            Id = detail.Id,
                            Status = EnumUtil.ParseEnum<WarrantyDetailStatus>(detail.Status),
                            CreateDate = detail.CreateDate,
                            StartDate = detail.StartDate,
                            Description = detail.Description,
                            Comments = detail.Comments,
                            WarrantyId = detail.WarrantyId,
                            AccountId = detail.AccountId
                        }).ToList(),
                        Customer = warranty.Inventory.OrderDetails
                            .Select(detail => detail.Order.Account)
                            .Where(account => account != null)
                            .Select(account => new AccountResponse
                            {
                                Id = account.Id,
                                FullName = account.FullName,
                                Role = EnumUtil.ParseEnum<RoleEnum>(account.Role),
                            }).FirstOrDefault()
                    },
                    predicate: x => x.Id.Equals(id),
                    include: x => x.Include(x => x.WarrantyDetails)
                                   .Include(x => x.Inventory)
                                       .ThenInclude(inventory => inventory.Machinery)
                                           .ThenInclude(machinery => machinery.Brand)
                                   .Include(x => x.Inventory)
                                       .ThenInclude(inventory => inventory.Machinery)
                                           .ThenInclude(machinery => machinery.Origin)
                                   .Include(x => x.Inventory)
                                       .ThenInclude(inventory => inventory.Machinery)
                                           .ThenInclude(machinery => machinery.Category)
                                   .Include(x => x.Inventory)
                                       .ThenInclude(inventory => inventory.Machinery)
                                           .ThenInclude(machinery => machinery.ImagesAlls)
                                   .Include(x => x.Inventory)
                                       .ThenInclude(inventory => inventory.Machinery)
                                           .ThenInclude(machinery => machinery.Specifications)
                                   .Include(x => x.Inventory)
                                       .ThenInclude(inventory => inventory.Machinery));

            if (warranty == null)
            {
                throw new BadHttpRequestException(MessageConstant.Warranty.WarrantyNotFoundMessage);
            }

            return warranty;
        }




        public Task<bool> RemoveWarrantyStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWarranty(Guid id, UpdateWarrantyRequest updateWarrantyRequest)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Warranty.EmptyWarrantyIdMessage);

            Warranty warranty = await _unitOfWork.GetRepository<Warranty>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Warranty.WarrantyNotFoundMessage);

            // Update warranty properties
            warranty.Status = updateWarrantyRequest.Status.GetDescriptionFromEnum(); ;
            warranty.Description = string.IsNullOrEmpty(updateWarrantyRequest.Description) ? updateWarrantyRequest.Description : warranty.Description;
            warranty.Comments = string.IsNullOrEmpty(updateWarrantyRequest.Comments) ? updateWarrantyRequest.Comments : warranty.Comments;
            warranty.NextMaintenanceDate = updateWarrantyRequest.NextMaintenanceDate.HasValue ? updateWarrantyRequest.NextMaintenanceDate.Value : warranty.NextMaintenanceDate;
            warranty.Priority = updateWarrantyRequest.Priority.HasValue ? updateWarrantyRequest.Priority.Value : warranty.Priority;

            _unitOfWork.GetRepository<Warranty>().UpdateAsync(warranty);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

    }
}
