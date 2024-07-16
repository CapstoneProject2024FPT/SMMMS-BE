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


namespace SAM.BusinessTier.Services.Implements
{
    public class MachineryService : BaseService<MachineryService>, IMachineryService
    {
        public MachineryService(IUnitOfWork<SamContext> unitOfWork, ILogger<MachineryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }
        public async Task<Guid> CreateNewMachinerys(CreateNewMachineryRequest request)
        {
            var currentUser = GetUsernameFromJwt();
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser));
            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            // Validate the CategoryId
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
                Status = MachineryStatus.Available.GetDescriptionFromEnum(),
                StockPrice = request.StockPrice,
                SellingPrice = request.SellingPrice,
                Priority = request.Priority,
                TimeWarranty = request.TimeWarranty,
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

            // Insert the new machinery and its specifications into the database
            await _unitOfWork.GetRepository<Machinery>().InsertAsync(newMachinery);
            await _unitOfWork.GetRepository<Specification>().InsertRangeAsync(specification);
            await _unitOfWork.GetRepository<ImagesAll>().InsertRangeAsync(imagesUrl);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Machinery.CreateNewMachineryFailedMessage);

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
                        SellingPrice = machinery.SellingPrice,
                        Priority = machinery.Priority,
                        TimeWarranty = machinery.TimeWarranty,
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

                        Quantity = machinery.Inventories.CountInventoryEachStatus()
                    },
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.Priority),
                    include: x => x.Include(x => x.Inventories)
                                   .Include(x => x.Brand)
                                   .Include(x => x.Origin)
                                   .Include(x => x.Category)
                                   .Include(x => x.ImagesAlls)
                                   .Include(x => x.Specifications))
                ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);

            return machineryList;
        }
        public async Task<IPaginate<GetMachinerySpecificationsRespone>> GetMachineryList(MachineryFilter filter, PagingModel pagingModel)
        {
            IPaginate<GetMachinerySpecificationsRespone> machineryList = await _unitOfWork.GetRepository<Machinery>().GetPagingListAsync
                (
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
                         Priority = x.Priority,
                         TimeWarranty = x.TimeWarranty,
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
                         Quantity = x.Inventories.CountInventoryEachStatus()
                     },
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.Priority),
                    include: x => x.Include(x => x.Inventories)
                                   .Include(x => x.Brand)
                                   .Include(x => x.Origin)
                                   .Include(x => x.Category)
                                   .Include(x => x.ImagesAlls)
                                   .Include(x => x.Specifications),
                    page: pagingModel.page,
                    size: pagingModel.size
                    ) ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage
                );


            return machineryList;
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
                               .Include(x => x.Specifications))
                ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);


            var getMachinerySpecificationsRespone = new GetMachinerySpecificationsRespone
            {
                Id = machinery.Id,
                Name = machinery.Name,
                Brand = new BrandResponse()
                {
                    Id = machinery.BrandId,
                    Name = machinery.Brand.Name,
                    Description = machinery.Brand.Description,
                },
                Model = machinery.Model,
                Description = machinery.Description,
                SellingPrice = machinery.SellingPrice,
                Priority = machinery.Priority,
                TimeWarranty = machinery.TimeWarranty,
                Status = EnumUtil.ParseEnum<MachineryStatus>(machinery.Status),
                CreateDate = machinery.CreateDate,
                Origin = new OriginResponse()
                {
                    Id = machinery.OriginId,
                    Name = machinery.Origin.Name,
                    Description = machinery.Origin.Description,
                },
                Category = new CategoryResponse()
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
                    

                // Set the inventory count here
                Quantity = machinery.Inventories.CountInventoryEachStatus()
            };

            return getMachinerySpecificationsRespone;
        }

        public async Task<GetMachineryAndComponentsResponse> GetMachineryAndComponentsByInventoryId(Guid id)
        {
            // Lấy thông tin Machinery từ Inventory
            var inventory = await _unitOfWork.GetRepository<Inventory>()
                .SingleOrDefaultAsync(
                    predicate: x => x.Id == id,
                    include: x => x.Include(i => i.Machinery)
                                   .Include(i => i.Machinery.Brand)
                                   .Include(i => i.Machinery.Origin)
                                   .Include(i => i.Machinery.Category)
                                   .Include(i => i.Machinery.ImagesAlls)
                                   .Include(i => i.Machinery.Specifications))
                ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);

            var machinery = inventory.Machinery;
            if (machinery == null)
            {
                throw new BadHttpRequestException("Không tìm thấy máy cơ khí cho số serial number này");
            }

            var machineryResponse = new GetMachinerySpecificationsRespone
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
                SellingPrice = machinery.SellingPrice,
                Priority = machinery.Priority,
                TimeWarranty = machinery.TimeWarranty,
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
                }).ToList()
            };

            // Lấy danh sách component inventories dựa trên MasterInventoryId
            var componentInventories = await _unitOfWork.GetRepository<Inventory>()
                .GetListAsync(
                    predicate: x => x.MasterInventoryId == id,
                    include: x => x.Include(i => i.MachineComponents))
                ?? throw new BadHttpRequestException("No component inventories found for the given master inventory ID.");

            var componentResponses = componentInventories.Select(component => new GetInventoryResponse
            {
                Id = component.Id,
                SerialNumber = component.SerialNumber,
                Status = component.Status,
                Type = component.Type,
                Condition = component.Condition,
                IsRepaired = component.IsRepaired,
                CreateDate = component.CreateDate,
                SoldDate = component.SoldDate,
                MachineComponentsId = component.MachineComponentsId,
                MasterInventoryId = component.MasterInventoryId
            }).ToList();

            var response = new GetMachineryAndComponentsResponse
            {
                Machinery = machineryResponse,
                Components = componentResponses
            };

            return response;
        }


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
            product.Status = updateProductRequest.Status.GetDescriptionFromEnum();
            product.Priority = updateProductRequest.Priority.HasValue ? updateProductRequest.Priority.Value : product.Priority;
            
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
