using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Extensions;
using SAM.BusinessTier.Payload.Address;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Districts;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.News;
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
    public class WarrantyService : BaseService<WarrantyService>, IWarrantyService
    {
        public WarrantyService(IUnitOfWork<SamContext> unitOfWork, ILogger<WarrantyService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewWarranty(CreateNewWarrantyRequest request)
        {
            var currentUser = GetUsernameFromJwt();
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser));
            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            var inventory = await _unitOfWork.GetRepository<Inventory>().SingleOrDefaultAsync(
                predicate: i => i.Id == request.InventoryId,
                include: i => i.Include(inv => inv.OrderDetails).ThenInclude(od => od.Order));

            if (inventory == null)
            {
                throw new BadHttpRequestException(MessageConstant.Inventory.NotFoundFailedMessage);
            }

            // Check completed
            var associatedOrder = inventory.OrderDetails.Select(od => od.Order).FirstOrDefault();
            if (associatedOrder == null || associatedOrder.Status != OrderStatus.Completed.GetDescriptionFromEnum())
            {
                throw new BadHttpRequestException("Không thể tạo yêu cầu bảo hành khi đơn hàng chưa được hoàn thành");
            }

            //var existingWarranty = await _unitOfWork.GetRepository<Warranty>().SingleOrDefaultAsync(
            //    predicate: w => w.InventoryId == request.InventoryId);

            //if (existingWarranty != null)
            //{
            //    throw new BadHttpRequestException("Đã có phiếu bảo trì tương tự trong ngày.");
            //}

            Warranty newWarranty = new Warranty
            {
                Id = Guid.NewGuid(),
                Type = WarrantyType.CustomerRequest.GetDescriptionFromEnum(),
                CreateDate = currentTime,
                StartDate = currentTime,
                Status = WarrantyStatus.Process.GetDescriptionFromEnum(),
                Description = request.Description,
                Priority = 1,
                InventoryId = request.InventoryId,
                AccountId = request.AccountId,
            };

            await _unitOfWork.GetRepository<Warranty>().InsertAsync(newWarranty);

            WarrantyDetail newWarrantyDetail = new WarrantyDetail
            {
                Id = Guid.NewGuid(),
                Type = newWarranty.Type,
                CreateDate = currentTime,
                StartDate = currentTime,
                Status = WarrantyDetailStatus.AwaitingAssignment.GetDescriptionFromEnum(),
                Description = $"Yêu cầu từ khách hàng - Bắt đầu từ {currentTime:yyyy-MM-dd HH:mm:ss}",
                WarrantyId = newWarranty.Id,
                AddressId = request.AddressId
            };

            await _unitOfWork.GetRepository<WarrantyDetail>().InsertAsync(newWarrantyDetail);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
            {
                throw new BadHttpRequestException(MessageConstant.WarrantyDetail.CreateNewWarrantyDetailFailedMessage);
            }

            return newWarranty.Id;
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
                        Customer = warranty.Inventory.OrderDetails
                            .Where(detail => detail.MachineryId == warranty.Inventory.Machinery.Id)
                            .Select(detail => new AccountResponse
                            {
                                Id = detail.Order.Account.Id,
                                FullName = detail.Order.Account.FullName,
                                Role = EnumUtil.ParseEnum<RoleEnum>(detail.Order.Account.Role),
                            }).FirstOrDefault(),
                        Address = warranty.Inventory.OrderDetails
                            .Select(detail => detail.Order.Address)
                            .Where(address => address != null)
                            .Select(address => new GetAddressResponse
                            {
                                Id = address.Id,
                                Name = address.Name,
                                Status = EnumUtil.ParseEnum<AddressStatus>(address.Status),
                                Note = address.Note,
                                NamePersonal = address.NamePersonal,
                                PhoneNumber = address.PhoneNumber,
                                City = address.City == null ? null : new CityResponse
                                {
                                    Id = address.City.Id,
                                    UnitId = address.City.UnitId,
                                    Name = address.City.Name
                                },
                                District = address.District == null ? null : new DistrictResponse
                                {
                                    Id = address.District.Id,
                                    UnitId = address.District.UnitId,
                                    Name = address.District.Name
                                },
                                Ward = address.Ward == null ? null : new WardResponse
                                {
                                    Id = address.Ward.Id,
                                    UnitId = address.Ward.UnitId,
                                    Name = address.Ward.Name
                                },
                                Account = address.Account == null ? null : new AccountResponse
                                {
                                    Id = address.Account.Id,
                                    FullName = address.Account.FullName,
                                    Role = EnumUtil.ParseEnum<RoleEnum>(address.Account.Role),
                                }
                            }).FirstOrDefault()
                    },
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.CreateDate),
                    include: x => x.Include(x => x.WarrantyDetails)
                           .Include(x => x.Inventory)
                               .ThenInclude(inventory => inventory.Machinery)
                           .Include(x => x.Inventory.OrderDetails)
                               .ThenInclude(orderDetail => orderDetail.Order.Address)
                               .ThenInclude(address => address.City)
                           .Include(x => x.Inventory.OrderDetails)
                               .ThenInclude(orderDetail => orderDetail.Order.Address)
                               .ThenInclude(address => address.District)
                           .Include(x => x.Inventory.OrderDetails)
                               .ThenInclude(orderDetail => orderDetail.Order.Address)
                               .ThenInclude(address => address.Ward)
                           .Include(x => x.Inventory.OrderDetails)
                               .ThenInclude(orderDetail => orderDetail.Order.Address)
                               .ThenInclude(address => address.Account)
                           .Include(x => x.Inventory.OrderDetails)
                               .ThenInclude(orderDetail => orderDetail.Order.Account))
                ?? throw new BadHttpRequestException(MessageConstant.Warranty.WarrantyNotFoundMessage);

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
                            }).FirstOrDefault(),
                        Address = warranty.Inventory.OrderDetails
                            .Select(detail => detail.Order.Address)
                            .Where(address => address != null)
                            .Select(address => new GetAddressResponse
                            {
                                Id = address.Id,
                                Name = address.Name,
                                Status = EnumUtil.ParseEnum<AddressStatus>(address.Status),
                                Note = address.Note,
                                NamePersonal = address.NamePersonal,
                                PhoneNumber = address.PhoneNumber,
                                City = address.City == null ? null : new CityResponse
                                {
                                    Id = address.City.Id,
                                    UnitId = address.City.UnitId,
                                    Name = address.City.Name
                                },
                                District = address.District == null ? null : new DistrictResponse
                                {
                                    Id = address.District.Id,
                                    UnitId = address.District.UnitId,
                                    Name = address.District.Name
                                },
                                Ward = address.Ward == null ? null : new WardResponse
                                {
                                    Id = address.Ward.Id,
                                    UnitId = address.Ward.UnitId,
                                    Name = address.Ward.Name
                                },
                                Account = address.Account == null ? null : new AccountResponse
                                {
                                    Id = address.Account.Id,
                                    FullName = address.Account.FullName,
                                    Role = EnumUtil.ParseEnum<RoleEnum>(address.Account.Role),
                                }
                            }).FirstOrDefault()
                            },
                            predicate: x => x.Id.Equals(id),
                            include: x => x.Include(x => x.WarrantyDetails)
                                           .Include(x => x.Inventory)
                                               .ThenInclude(inventory => inventory.Machinery)
                                           .Include(x => x.Inventory.OrderDetails)
                                               .ThenInclude(orderDetail => orderDetail.Order.Address)
                                               .ThenInclude(address => address.City)
                                           .Include(x => x.Inventory.OrderDetails)
                                               .ThenInclude(orderDetail => orderDetail.Order.Address)
                                               .ThenInclude(address => address.District)
                                           .Include(x => x.Inventory.OrderDetails)
                                               .ThenInclude(orderDetail => orderDetail.Order.Address)
                                               .ThenInclude(address => address.Ward)
                                           .Include(x => x.Inventory.OrderDetails)
                                               .ThenInclude(orderDetail => orderDetail.Order.Address)
                                               .ThenInclude(address => address.Account)
                                           .Include(x => x.Inventory.OrderDetails)
                                               .ThenInclude(orderDetail => orderDetail.Order.Account))
                ?? throw new BadHttpRequestException(MessageConstant.Warranty.WarrantyNotFoundMessage);

            return warranty;
        }




        public Task<bool> RemoveWarrantyStatus(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWarranty(Guid id, UpdateWarrantyRequest updateWarrantyRequest)
        {
            DateTime currentTime = TimeUtils.GetCurrentSEATime();
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Warranty.EmptyWarrantyIdMessage);

            Warranty warranty = await _unitOfWork.GetRepository<Warranty>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Warranty.WarrantyNotFoundMessage);

            
            warranty.Description = !string.IsNullOrEmpty(updateWarrantyRequest.Description) ? updateWarrantyRequest.Description : warranty.Description;
            warranty.Comments = !string.IsNullOrEmpty(updateWarrantyRequest.Comments) ? updateWarrantyRequest.Comments : warranty.Comments;
            warranty.Priority = updateWarrantyRequest.Priority.HasValue ? updateWarrantyRequest.Priority.Value : warranty.Priority;
            if (!updateWarrantyRequest.Status.HasValue)
            {
                throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
            }
            else
            {
                warranty.Status = updateWarrantyRequest.Status.GetDescriptionFromEnum();
            }
            _unitOfWork.GetRepository<Warranty>().UpdateAsync(warranty);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            
            if (isSuccess)
            {
                var warrantyDetails = await _unitOfWork.GetRepository<WarrantyDetail>().GetListAsync(
                    predicate: wd => wd.WarrantyId.Equals(id) && wd.Status != WarrantyDetailStatus.Completed.GetDescriptionFromEnum());

                if (!warrantyDetails.Any())
                {
                    warranty.Status = WarrantyStatus.Completed.GetDescriptionFromEnum();
                    warranty.CompletionDate = currentTime;
                    _unitOfWork.GetRepository<Warranty>().UpdateAsync(warranty);
                    isSuccess = await _unitOfWork.CommitAsync() > 0;
                }
            }

            return isSuccess;
        }


    }
}
