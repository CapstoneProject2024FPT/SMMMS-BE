using AutoMapper;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Paginate;
using SAM.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.Order;
using Azure.Core;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Extensions;
using Microsoft.EntityFrameworkCore;
using Azure;
using SAM.BusinessTier.Payload.User;
using System.ComponentModel;


namespace SAM.BusinessTier.Services.Implements
{
    public class MachineryService : BaseService<MachineryService>, IMachineryService
    {
        public MachineryService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<MachineryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }
        public async Task<bool> AddComponentToMachinery(Guid id, List<Guid> request)
        {

            // Retrieve the account or throw an exception if not found
            Machinery machinery = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);

            var soldInventories = await _unitOfWork.GetRepository<Inventory>().GetListAsync(
                predicate: x => x.MachineryId == id && x.Status == InventoryStatus.Sold.GetDescriptionFromEnum());

            if (soldInventories.Any())
            {
                throw new BadHttpRequestException(MessageConstant.Inventory.AlreadySoldMessage);
            }
            // Retrieve current rank IDs associated with the account
            List<Guid> currentMachineryComponentPartIds = (List<Guid>)await _unitOfWork.GetRepository<MachineryComponentPart>().GetListAsync(
                selector: x => x.MachineComponentsId,
                predicate: x => x.MachineryId.Equals(id));

            // Determine the IDs to add, remove, and keep
            (List<Guid> idsToRemove, List<Guid> idsToAdd, List<Guid> idsToKeep) splittedComponentIds =
                CustomListUtil.splitidstoaddandremove(currentMachineryComponentPartIds, request);

            // Add new Component
            if (splittedComponentIds.idsToAdd.Count > 0)
            {
                List<MachineryComponentPart> ComponentToInserts = new List<MachineryComponentPart>();
                splittedComponentIds.idsToAdd.ForEach(rankId => ComponentToInserts.Add(new MachineryComponentPart
                {
                    Id = Guid.NewGuid(),
                    MachineryId = id,
                    MachineComponentsId = rankId, 
                }));
                await _unitOfWork.GetRepository<MachineryComponentPart>().InsertRangeAsync(ComponentToInserts);
            }

            // Remove obsolete ranks
            if (splittedComponentIds.idsToRemove.Count > 0)
            {
                List<MachineryComponentPart> ranksToDelete = (List<MachineryComponentPart>)await _unitOfWork.GetRepository<MachineryComponentPart>()
                    .GetListAsync(predicate: x =>
                        x.MachineryId.Equals(id) &&
                        splittedComponentIds.idsToRemove.Contains(x.MachineComponentsId));

                _unitOfWork.GetRepository<MachineryComponentPart>().DeleteRangeAsync(ranksToDelete);
            }

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
        public async Task<Guid> CreateNewMachinerys(CreateNewMachineryRequest request)
        {
            var currentUser = GetUsernameFromJwt();
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser));
            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            if (request.CategoryId.HasValue)
            {
                var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                    predicate: c => c.Id == request.CategoryId.Value);
                if (category == null)
                {
                    throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
                }
            }
            if (request.OriginId.HasValue)
            {
                var origin = await _unitOfWork.GetRepository<Origin>().SingleOrDefaultAsync(
                    predicate: c => c.Id == request.OriginId.Value);
                if (origin == null)
                {
                    throw new BadHttpRequestException(MessageConstant.Origin.NotFoundFailedMessage);
                }
            }
            if (request.BrandId.HasValue)
            {
                var brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(
                    predicate: c => c.Id == request.BrandId.Value);
                if (brand == null)
                {
                    throw new BadHttpRequestException(MessageConstant.Brand.NotFoundFailedMessage);
                }
            }

            Machinery newMachinery = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                OriginId = request.OriginId,
                BrandId = request.BrandId,
                Model = request.Model,
                Description = request.Description,
                Status = MachineryStatus.OutOfStock.GetDescriptionFromEnum(),
                StockPrice = request.StockPrice,
                SellingPrice = request.SellingPrice,
                Priority = request.Priority,
                TimeWarranty = request.TimeWarranty,
                MonthWarrantyNumber = request.MonthWarrantyNumber,
                CategoryId = request.CategoryId,
                CreateDate = DateTime.Now,

            };

            var specification = new List<Specification>();
            foreach (var spec in request.SpecificationList)
            {
                specification.Add(new Specification
                {
                    Id = Guid.NewGuid(),
                    MachineryId = newMachinery.Id,
                    Name = spec.Name,
                    Value = spec.Value,
                });
            };
            var imagesUrl = new List<ImagesAll>();
            foreach (var img in request.Image)
            {
                imagesUrl.Add(new ImagesAll
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = img.ImageURL,
                    CreateDate = DateTime.Now,
                    MachineryId = newMachinery.Id,

                });
            };

            await _unitOfWork.GetRepository<Machinery>().InsertAsync(newMachinery);
            await _unitOfWork.GetRepository<Specification>().InsertRangeAsync(specification);
            await _unitOfWork.GetRepository<ImagesAll>().InsertRangeAsync(imagesUrl);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Machinery.CreateNewMachineryFailedMessage);
            if (request.MachineComponentsId != null && request.MachineComponentsId.Count > 0)
            {
                bool componentsAdded = await AddComponentToMachinery(newMachinery.Id, request.MachineComponentsId);
                if (!componentsAdded)
                {
                    throw new BadHttpRequestException(MessageConstant.MachineryComponents.CreateNewMachineryComponentsFailedMessage);
                }
            }
            return newMachinery.Id;
        }



        public async Task<ICollection<GetMachinerySpecificationsRespone>> GetMachineryListNoPagingNate(MachineryFilter filter)
        {
            var machineryList = await _unitOfWork.GetRepository<Machinery>()
                .GetListAsync(
                    selector: machinery => new GetMachinerySpecificationsRespone
                    {
                        Id = machinery.Id,
                        Name = machinery.Name,
                        Brand = new BrandResponse
                        {
                            Id = machinery.BrandId,
                            Name = machinery.Brand.Name,
                            Description = machinery.Brand.Description,
                        },
                        Model = machinery.Model,
                        Description = machinery.Description,
                        StockPrice =  machinery.StockPrice,
                        SellingPrice = machinery.SellingPrice,
                        FinalAmount = machinery.SellingPrice,
                        Priority = machinery.Priority,
                        TimeWarranty = machinery.TimeWarranty,
                        MonthWarrantyNumber = machinery.MonthWarrantyNumber,
                        Status = EnumUtil.ParseEnum<MachineryStatus>(machinery.Status),
                        CreateDate = machinery.CreateDate,
                        Origin = new OriginResponse
                        {
                            Id = machinery.OriginId,
                            Name = machinery.Origin.Name,
                            Description = machinery.Origin.Description,
                        },
                        Category = new CategoryResponse
                        {
                            Id = machinery.CategoryId,
                            Name = machinery.Category.Name,
                            Type = EnumUtil.ParseEnum<CategoryType>(machinery.Category.Type),
                        },
                        Component = machinery.MachineryComponentParts.Select(part => new ComponentResponse
                        {
                            Id = part.MachineComponents.Id,
                            Name = part.MachineComponents.Name,
                            Description = part.MachineComponents.Description,
                            Status = EnumUtil.ParseEnum<ComponentStatus>(part.MachineComponents.Status),
                            StockPrice = part.MachineComponents.StockPrice,
                            SellingPrice = part.MachineComponents.SellingPrice
                        }).ToList(),
                        Image = machinery.ImagesAlls.Select(image => new MachineryImagesResponse
                        {
                            Id = image.Id,
                            ImageURL = image.ImageUrl,
                            CreateDate = image.CreateDate
                        }).ToList(),
                        Specifications = machinery.Specifications.Select(spec => new SpecificationsResponse
                        {
                            SpecificationId = spec.Id,
                            MachineryId = spec.MachineryId,
                            Name = spec.Name,
                            Value = spec.Value
                        }).ToList(),
                        Quantity = machinery.Inventories.CountInventoryEachStatus(),

                    },
                    filter: filter,
                    orderBy: x => x.OrderByDescending(x => x.CreateDate),
                    include: x => x.Include(x => x.Inventories)
                                   .Include(x => x.Brand)
                                   .Include(x => x.Origin)
                                   .Include(x => x.Category)
                                   .Include(x => x.ImagesAlls)
                                   .Include(x => x.Specifications)
                                   .Include(x => x.MachineryComponentParts)
                                   .ThenInclude(part => part.MachineComponents))
                ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);
            // Tính toán FinalAmount cho từng máy móc trong danh sách
            foreach (var machinery in machineryList)
            {
                var (finalAmount, discountValue1) = await CalculateFinalAmount((Guid)machinery.Id, machinery.SellingPrice, machinery.Category.Id);
                machinery.FinalAmount = finalAmount;
                machinery.Discount = (int?)discountValue1;
            }

            return machineryList;
        }

        public async Task<IPaginate<GetMachinerySpecificationsRespone>> GetMachineryList(MachineryFilter filter, PagingModel pagingModel)
        {
            var machineryList = await _unitOfWork.GetRepository<Machinery>().GetPagingListAsync(
                selector: x => new GetMachinerySpecificationsRespone
                {
                    Id = x.Id,
                    Name = x.Name,
                    Brand = new BrandResponse
                    {
                        Id = x.BrandId,
                        Name = x.Brand.Name,
                        Description = x.Brand.Description,
                    },
                    Model = x.Model,
                    Description = x.Description,
                    SellingPrice = x.SellingPrice,
                    StockPrice = x.StockPrice,
                    Priority = x.Priority,
                    TimeWarranty = x.TimeWarranty,
                    MonthWarrantyNumber = x.MonthWarrantyNumber,
                    Status = EnumUtil.ParseEnum<MachineryStatus>(x.Status),
                    CreateDate = x.CreateDate,
                    Origin = new OriginResponse
                    {
                        Id = x.OriginId,
                        Name = x.Origin.Name,
                        Description = x.Origin.Description,
                    },
                    Category = new CategoryResponse
                    {
                        Id = x.CategoryId,
                        Name = x.Category.Name,
                        Type = EnumUtil.ParseEnum<CategoryType>(x.Category.Type),
                    },
                    Component = x.MachineryComponentParts.Select(part => new ComponentResponse
                    {
                        Id = part.MachineComponents.Id,
                        Name = part.MachineComponents.Name,
                        Description = part.MachineComponents.Description,
                        Status = EnumUtil.ParseEnum<ComponentStatus>(part.MachineComponents.Status),
                        StockPrice = part.MachineComponents.StockPrice,
                        SellingPrice = part.MachineComponents.SellingPrice
                    }).ToList(),
                    Image = x.ImagesAlls.Select(image => new MachineryImagesResponse
                    {
                        Id = image.Id,
                        ImageURL = image.ImageUrl,
                        CreateDate = image.CreateDate
                    }).ToList(),
                    Specifications = x.Specifications.Select(spec => new SpecificationsResponse
                    {
                        SpecificationId = spec.Id,
                        MachineryId = spec.MachineryId,
                        Name = spec.Name,
                        Value = spec.Value
                    }).ToList(),
                    Quantity = x.Inventories.CountInventoryEachStatus(),

                    // FinalAmount sẽ được tính toán sau khi có danh sách.
                    FinalAmount = x.SellingPrice
                },
                filter: filter,
                orderBy: x => x.OrderByDescending(x => x.CreateDate),
                include: x => x.Include(x => x.Inventories)
                              .Include(x => x.Brand)
                              .Include(x => x.Origin)
                              .Include(x => x.Category)
                              .Include(x => x.ImagesAlls)
                              .Include(x => x.Specifications)
                              .Include(x => x.MachineryComponentParts)
                              .ThenInclude(part => part.MachineComponents),
                page: pagingModel.page,
                size: pagingModel.size
            ) ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);

            // Tính toán FinalAmount cho từng máy móc trong danh sách
            foreach (var machinery in machineryList.Items)
            {
                var (finalAmount, discountValue1) = await CalculateFinalAmount((Guid)machinery.Id, machinery.SellingPrice, machinery.Category.Id);
                machinery.FinalAmount = finalAmount;
                machinery.Discount = (int?)discountValue1;
            }

            return machineryList;
        }

        // Hàm tính toán FinalAmount
        private async Task<(double? finalAmount, double? discountValue)> CalculateFinalAmount(Guid machineryId, double? sellingPrice, Guid? categoryId)
        {
            double? finalAmount = sellingPrice;
            int? discountValue1 = 0;
            if (sellingPrice.HasValue && categoryId.HasValue)
            {
                var discountCategory = await _unitOfWork.GetRepository<DiscountCategory>().SingleOrDefaultAsync(
                    predicate: dc => dc.CategoryId == categoryId);

                if (discountCategory != null)
                {
                    var discount = await _unitOfWork.GetRepository<Discount>().SingleOrDefaultAsync(
                        predicate: d => d.Id == discountCategory.DiscountId);

                    if (discount != null && discount.Value.HasValue)
                    {
                        double discountValue = (discount.Value.Value / 100.0) * sellingPrice.Value;
                        finalAmount = sellingPrice - discountValue;
                        discountValue1 = discount.Value.Value;
                    }
                }
            }

            return (finalAmount, discountValue1);
        }





        public async Task<GetMachinerySpecificationsRespone> GetMachinerySpecificationsDetail(Guid id)
        {
            var machinery = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Inventories)
                               .Include(x => x.Brand)
                               .Include(x => x.Origin)
                               .Include(x => x.Category)
                               .Include(x => x.ImagesAlls)
                               .Include(x => x.Specifications)
                               .Include(x => x.MachineryComponentParts)
                               .ThenInclude(part => part.MachineComponents))
                ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);

            double? finalAmount = machinery.SellingPrice;
            var discountNumber = 0;
            // Kiểm tra nếu SellingPrice không bị null
            if (machinery.SellingPrice.HasValue && machinery.CategoryId.HasValue)
            {
                var discountCategory = await _unitOfWork.GetRepository<DiscountCategory>().SingleOrDefaultAsync(
                    predicate: dc => dc.CategoryId == machinery.CategoryId);

                if (discountCategory != null)
                {
                    var discount = await _unitOfWork.GetRepository<Discount>().SingleOrDefaultAsync(
                        predicate: d => d.Id == discountCategory.DiscountId);

                    // Kiểm tra nếu discount không bị null
                    if (discount != null)
                    {
                        // Kiểm tra nếu discount.Value không bị null
                        if (discount.Value.HasValue)
                        {
                            double discountValue = (discount.Value.Value / 100.0) * machinery.SellingPrice.Value;
                            finalAmount = machinery.SellingPrice.Value - discountValue;
                            discountNumber = (int)discount.Value.Value;
                        }
                        else
                        {
                            // Nếu discount.Value bị null, giữ FinalAmount là SellingPrice
                            finalAmount = machinery.SellingPrice;
                            discountNumber = (int)discount.Value.Value;
                        }
                    }
                }
            }

            var getMachinerySpecificationsRespone = new GetMachinerySpecificationsRespone
            {
                Id = machinery.Id,
                Name = machinery.Name,
                Brand = new BrandResponse
                {
                    Id = machinery.BrandId,
                    Name = machinery.Brand.Name,
                    Description = machinery.Brand.Description,
                },
                Model = machinery.Model,
                Description = machinery.Description,
                StockPrice = machinery.StockPrice,
                SellingPrice = machinery.SellingPrice,
                FinalAmount = finalAmount,
                Discount = discountNumber,
                Priority = machinery.Priority,
                TimeWarranty = machinery.TimeWarranty,
                MonthWarrantyNumber = machinery.MonthWarrantyNumber,
                Status = EnumUtil.ParseEnum<MachineryStatus>(machinery.Status),
                CreateDate = machinery.CreateDate,
                Origin = new OriginResponse
                {
                    Id = machinery.OriginId,
                    Name = machinery.Origin.Name,
                    Description = machinery.Origin.Description,
                },
                Category = new CategoryResponse
                {
                    Id = machinery.CategoryId,
                    Name = machinery.Category.Name,
                    Type = EnumUtil.ParseEnum<CategoryType>(machinery.Category.Type),
                },
                Image = machinery.ImagesAlls.Select(image => new MachineryImagesResponse
                {
                    Id = image.Id,
                    ImageURL = image.ImageUrl,
                    CreateDate = image.CreateDate
                }).ToList(),
                Specifications = machinery.Specifications.Select(spec => new SpecificationsResponse
                {
                    SpecificationId = spec.Id,
                    MachineryId = spec.MachineryId,
                    Name = spec.Name,
                    Value = spec.Value
                }).ToList(),
                Quantity = machinery.Inventories.CountInventoryEachStatus(),

                Component = machinery.MachineryComponentParts.Select(part => new ComponentResponse
                {
                    Id = part.MachineComponents.Id,
                    Name = part.MachineComponents.Name,
                    Description = part.MachineComponents.Description,
                    Status = EnumUtil.ParseEnum<ComponentStatus>(part.MachineComponents.Status),
                    StockPrice = part.MachineComponents.StockPrice,
                    SellingPrice = part.MachineComponents.SellingPrice
                }).ToList()
            };

            return getMachinerySpecificationsRespone;
        }


        //public async Task<GetMachineryAndComponentsFollowInventoryIdResponse> GetMachineryAndComponentsByInventoryId(Guid id)
        //{
        //    // Lấy thông tin Machinery từ Inventory
        //    var inventory = await _unitOfWork.GetRepository<Inventory>()
        //        .SingleOrDefaultAsync(
        //            predicate: x => x.Id == id,
        //            include: x => x.Include(i => i.Machinery)
        //                           .Include(i => i.Machinery.Brand)
        //                           .Include(i => i.Machinery.Origin)
        //                           .Include(i => i.Machinery.Category)
        //                           .Include(i => i.Machinery.ImagesAlls)
        //                           .Include(i => i.Machinery.Specifications))
        //        ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);

        //    var machinery = inventory.Machinery;
        //    if (machinery == null)
        //    {
        //        throw new BadHttpRequestException("Không tìm thấy máy cơ khí cho số định dành này");
        //    }

        //    var machineryResponse = new GetMachinerySpecificationsRespone
        //    {
        //        Id = machinery.Id,
        //        Name = machinery.Name,
        //        Brand = new BrandResponse
        //        {
        //            Id = machinery.BrandId,
        //            Name = machinery.Brand.Name,
        //            Description = machinery.Brand.Description,
        //        },
        //        Model = machinery.Model,
        //        Description = machinery.Description,
        //        StockPrice = machinery.StockPrice,
        //        SellingPrice = machinery.SellingPrice,
        //        Priority = machinery.Priority,
        //        TimeWarranty = machinery.TimeWarranty,
        //        MonthWarrantyNumber = machinery.MonthWarrantyNumber,
        //        Status = EnumUtil.ParseEnum<MachineryStatus>(machinery.Status),
        //        CreateDate = machinery.CreateDate,
        //        Origin = new OriginResponse
        //        {
        //            Id = machinery.OriginId,
        //            Name = machinery.Origin.Name,
        //            Description = machinery.Origin.Description,
        //        },
        //        Category = new CategoryResponse
        //        {
        //            Id = machinery.CategoryId,
        //            Name = machinery.Category.Name,
        //            Type = EnumUtil.ParseEnum<CategoryType>(machinery.Category.Type),
        //        },
        //        Image = machinery.ImagesAlls.Select(image => new MachineryImagesResponse
        //        {
        //            Id = image.Id,
        //            ImageURL = image.ImageUrl,
        //            CreateDate = image.CreateDate
        //        }).ToList(),
        //        Specifications = machinery.Specifications.Select(spec => new SpecificationsResponse
        //        {
        //            SpecificationId = spec.Id,
        //            MachineryId = spec.MachineryId,
        //            Name = spec.Name,
        //            Value = spec.Value
        //        }).ToList()
        //    };

        //    // Lấy danh sách component inventories dựa trên MasterInventoryId
        //    var componentInventories = await _unitOfWork.GetRepository<Inventory>()
        //        .GetListAsync(
        //            predicate: x => x.MasterInventoryId == id,
        //            include: x => x.Include(i => i.MachineComponents))
        //        ?? throw new BadHttpRequestException("Không tìm thấy sản phẩm theo mã sản phẩm");

        //    var componentResponses = componentInventories.Select(component => new GetInventoryResponse
        //    {
        //        Id = component.Id,
        //        SerialNumber = component.SerialNumber,
        //        Status = component.Status,
        //        Type = component.Type,
        //        Condition = component.Condition,
        //        IsRepaired = component.IsRepaired,
        //        CreateDate = component.CreateDate,
        //        SoldDate = component.SoldDate,
        //        MachineComponentsId = component.MachineComponentsId,
        //        MasterInventoryId = component.MasterInventoryId
        //    }).ToList();

        //    var response = new GetMachineryAndComponentsFollowInventoryIdResponse
        //    {
        //        Machinery = machineryResponse,
        //        Components = componentResponses
        //    };

        //    return response;
        //}


        public async Task<bool> RemoveMachineryStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Machinery.EmptyMachineryIdMessage);
            Machinery product = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);
            product.Status = MachineryStatus.UnAvailable.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Machinery>().UpdateAsync(product);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateMachinery(Guid id, UpdateMachineryRequest updateProductRequest)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Machinery.EmptyMachineryIdMessage);
            Machinery product = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNameExisted);

            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateProductRequest.CategoryId))
            ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);

            Brand brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateProductRequest.BrandId))
            ?? throw new BadHttpRequestException(MessageConstant.Brand.NotFoundFailedMessage);

            Origin origin = await _unitOfWork.GetRepository<Origin>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateProductRequest.OriginId))
            ?? throw new BadHttpRequestException(MessageConstant.Origin.NotFoundFailedMessage);

            product.Category = category;
            product.Brand = brand;
            product.Origin = origin;

            product.Name = string.IsNullOrEmpty(updateProductRequest.Name) ? product.Name : updateProductRequest.Name;
            product.Model = string.IsNullOrEmpty(updateProductRequest.Model) ? product.Model : updateProductRequest.Model;
            product.SellingPrice = updateProductRequest.SellingPrice.HasValue ? updateProductRequest.SellingPrice.Value : product.SellingPrice;
            product.StockPrice = updateProductRequest.StockPrice.HasValue ? updateProductRequest.StockPrice.Value : product.StockPrice;
            product.Description = string.IsNullOrEmpty(updateProductRequest.Description) ? product.Description : updateProductRequest.Description;
            product.TimeWarranty = updateProductRequest.TimeWarranty.HasValue ? updateProductRequest.TimeWarranty.Value : product.TimeWarranty;
            product.MonthWarrantyNumber = updateProductRequest.MonthWarrantyNumber.HasValue ? updateProductRequest.MonthWarrantyNumber.Value : product.MonthWarrantyNumber;
            
            product.Priority = updateProductRequest.Priority.HasValue ? updateProductRequest.Priority.Value : product.Priority;
            if (!updateProductRequest.Status.HasValue)
            {
                throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
            }
            else
            {
                product.Status = updateProductRequest.Status.GetDescriptionFromEnum();
            }

            _unitOfWork.GetRepository<Machinery>().UpdateAsync(product);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

        public async Task<bool> UpdateStatusMachineryResponse(Guid id, UpdateStatusMachineryResponse updateStatusMachineryResponse)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Machinery.EmptyMachineryIdMessage);
            Machinery product = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNameExisted);

            product.Status = MachineryStatus.Available.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<Machinery>().UpdateAsync(product);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }
    }
}
