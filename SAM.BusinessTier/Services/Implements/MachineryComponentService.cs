using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Extensions;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Inventory;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Payload.MachineryComponent;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Paginate;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SAM.BusinessTier.Services.Implements
{
    public class MachineryComponentService : BaseService<MachineryComponentService>, IMachineryComponentService
    {
        public MachineryComponentService(IUnitOfWork<SamContext> unitOfWork, ILogger<MachineryComponentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewMachineryComponents(CreateNewMachineryComponentRequest createMachineryComponentRequest)
        {
            var currentUser = GetUsernameFromJwt();
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser));
            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            // Validate the CategoryId
            if (createMachineryComponentRequest.CategoryId.HasValue)
            {
                var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                    predicate: c => c.Id == createMachineryComponentRequest.CategoryId.Value);
                if (category == null)
                {
                    throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
                }
            }
            if (createMachineryComponentRequest.OriginId.HasValue)
            {
                var origin = await _unitOfWork.GetRepository<Origin>().SingleOrDefaultAsync(
                    predicate: c => c.Id == createMachineryComponentRequest.OriginId.Value);
                if (origin == null)
                {
                    throw new BadHttpRequestException(MessageConstant.Origin.NotFoundFailedMessage);
                }
            }
            if (createMachineryComponentRequest.BrandId.HasValue)
            {
                var brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(
                    predicate: c => c.Id == createMachineryComponentRequest.BrandId.Value);
                if (brand == null)
                {
                    throw new BadHttpRequestException(MessageConstant.Brand.NotFoundFailedMessage);
                }
            }

            MachineComponent newComponent = new()
            {
                Id = Guid.NewGuid(),
                Name = createMachineryComponentRequest.Name,
                Description = createMachineryComponentRequest.Description,
                Status = ComponentStatus.Active.GetDescriptionFromEnum(),
                StockPrice = createMachineryComponentRequest.StockPrice,
                SellingPrice = createMachineryComponentRequest.SellingPrice,
                TimeWarranty = createMachineryComponentRequest.TimeWarranty,
                CategoryId = createMachineryComponentRequest.CategoryId,
                BrandId = createMachineryComponentRequest.BrandId,
                OriginId = createMachineryComponentRequest.OriginId,
                CreateDate = DateTime.Now
            };

            var imagesUrl = new List<ImageComponent>();
            foreach (var img in createMachineryComponentRequest.Image)
            {
                imagesUrl.Add(new ImageComponent
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = img.ImageUrl,
                    CreateDate = DateTime.Now,
                    MachineComponentId = newComponent.Id,
                });
            }

            // Insert the new component and its images into the database
            await _unitOfWork.GetRepository<MachineComponent>().InsertAsync(newComponent);
            await _unitOfWork.GetRepository<ImageComponent>().InsertRangeAsync(imagesUrl);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.MachineryComponents.CreateNewMachineryComponentsFailedMessage);

            return newComponent.Id;
        }

        public async Task<GetMachineryComponentResponse> GetMachineryComponentById(Guid id)
        {
            var component = await _unitOfWork.GetRepository<MachineComponent>()
                .SingleOrDefaultAsync(
                    selector: component => new GetMachineryComponentResponse
                    {
                        Id = component.Id,
                        Name = component.Name,
                        Description = component.Description,
                        CreateDate = component.CreateDate,
                        Status = component.Status != null ? EnumUtil.ParseEnum<ComponentStatus>(component.Status) : null,
                        StockPrice = component.StockPrice,
                        SellingPrice = component.SellingPrice,
                        TimeWarranty = component.TimeWarranty,
                        Origin = new OriginResponse
                        {
                            Id = component.OriginId,
                            Name = component.Origin.Name,
                            Description = component.Origin.Description,
                        },
                        Brand = new BrandResponse
                        {
                            Id = component.BrandId,
                            Name = component.Brand.Name,
                            Description = component.Brand.Description,
                        },
                        Category = new CategoryResponse
                        {
                            Id = component.CategoryId,
                            Name = component.Category.Name,
                            Type = component.Category.Type != null ? EnumUtil.ParseEnum<CategoryType>(component.Category.Type) : null,
                        },
                        Image = component.ImageComponents.Select(image => new ImageComponentResponse
                        {
                            Id = image.Id,
                            ImageURL = image.ImageUrl,
                            CreateDate = image.CreateDate
                        }).ToList(),
                        Quantity = component.Inventories.CountInventoryEachStatus()
                    },
                    predicate: x => x.Id == id,
                    include: x => x.Include(x => x.Inventories)
                                   .Include(x => x.Brand)
                                   .Include(x => x.Origin)
                                   .Include(x => x.Category)
                                   .Include(x => x.ImageComponents));

            if (component == null)
            {
                throw new BadHttpRequestException(MessageConstant.MachineryComponents.MachineryComponentsNotFoundMessage);
            }

            return component;
        }



        public async Task<IPaginate<GetMachineryComponentResponse>> GetMachineryComponentList(MachineryComponentFilter filter, PagingModel pagingModel)
        {
            var componentList = await _unitOfWork.GetRepository<MachineComponent>()
                .GetPagingListAsync(
                    selector: component => new GetMachineryComponentResponse
                    {
                        Id = component.Id,
                        Name = component.Name,
                        Description = component.Description,
                        CreateDate = component.CreateDate,
                        Status = component.Status != null ? EnumUtil.ParseEnum<ComponentStatus>(component.Status) : null,
                        StockPrice = component.StockPrice,
                        SellingPrice = component.SellingPrice,
                        TimeWarranty = component.TimeWarranty,
                        Origin = new OriginResponse
                        {
                            Id = component.OriginId,
                            Name = component.Origin.Name,
                            Description = component.Origin.Description,
                        },
                        Brand = new BrandResponse
                        {
                            Id = component.BrandId,
                            Name = component.Brand.Name,
                            Description = component.Brand.Description,
                        },
                        Category = new CategoryResponse
                        {
                            Id = component.CategoryId,
                            Name = component.Category.Name,
                            Type = component.Category.Type != null ? EnumUtil.ParseEnum<CategoryType>(component.Category.Type) : null,
                        },
                        Image = component.ImageComponents.Select(image => new ImageComponentResponse
                        {
                            Id = image.Id,
                            ImageURL = image.ImageUrl,
                            CreateDate = image.CreateDate
                        }).ToList(),
                        Quantity = component.Inventories.CountInventoryEachStatus()
                    },
                    filter: filter,
                    orderBy: x => x.OrderByDescending(x => x.CreateDate),
                    include: x => x.Include(x => x.Inventories)
                                   .Include(x => x.Brand)
                                   .Include(x => x.Origin)
                                   .Include(x => x.Category)
                                   .Include(x => x.ImageComponents),
                    page: pagingModel.page,
                    size: pagingModel.size
                ) ?? throw new BadHttpRequestException(MessageConstant.MachineryComponents.MachineryComponentsNotFoundMessage);

            return componentList;
        }

        public async Task<ICollection<GetMachineryComponentResponse>> GetMachineryComponentListNoPagingNate(MachineryComponentFilter filter)
        {
            var componentList = await _unitOfWork.GetRepository<MachineComponent>()
                .GetListAsync(
                    selector: component => new GetMachineryComponentResponse
                    {
                        Id = component.Id,
                        Name = component.Name,
                        Description = component.Description,
                        CreateDate = component.CreateDate,
                        Status = component.Status != null ? EnumUtil.ParseEnum<ComponentStatus>(component.Status) : null,
                        StockPrice = component.StockPrice,
                        SellingPrice = component.SellingPrice,
                        TimeWarranty = component.TimeWarranty,
                        Origin = new OriginResponse
                        {
                            Id = component.OriginId,
                            Name = component.Origin.Name,
                            Description = component.Origin.Description,
                        },
                        Brand = new BrandResponse
                        {
                            Id = component.BrandId,
                            Name = component.Brand.Name,
                            Description = component.Brand.Description,
                        },
                        Category = new CategoryResponse
                        {
                            Id = component.CategoryId,
                            Name = component.Category.Name,
                            Type = component.Category.Type != null ? EnumUtil.ParseEnum<CategoryType>(component.Category.Type) : null,
                        },
                        Image = component.ImageComponents.Select(image => new ImageComponentResponse
                        {
                            Id = image.Id,
                            ImageURL = image.ImageUrl,
                            CreateDate = image.CreateDate
                        }).ToList(),
                        Quantity = component.Inventories.CountInventoryEachStatus()
                    },
                    filter: filter,
                    orderBy: x => x.OrderByDescending(x => x.CreateDate), // Adjust as per your sorting requirements
                    include: x => x.Include(x => x.Inventories)
                                   .Include(x => x.Brand)
                                   .Include(x => x.Origin)
                                   .Include(x => x.Category)
                                   .Include(x => x.ImageComponents))
                ?? throw new BadHttpRequestException(MessageConstant.MachineryComponents.MachineryComponentsNotFoundMessage);

            return componentList;
        }

        public async Task<bool> RemoveMachineryComponentStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.MachineryComponents.EmptyMachineryComponentsIdMessage);

            var component = await _unitOfWork.GetRepository<MachineComponent>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.MachineryComponents.MachineryComponentsNotFoundMessage);

            // Remove status logic, assuming status is nullable
            component.Status = null; // Or set to a default status as needed

            _unitOfWork.GetRepository<MachineComponent>().UpdateAsync(component);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }


        public async Task<bool> UpdateMachineryComponent(Guid id, UpdateMachineryComponentRequest updateComponentRequest)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.MachineryComponents.EmptyMachineryComponentsIdMessage);

            var component = await _unitOfWork.GetRepository<MachineComponent>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.MachineryComponents.MachineryComponentsNotFoundMessage);

            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateComponentRequest.CategoryId))
                ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);

            Brand brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateComponentRequest.BrandId))
                ?? throw new BadHttpRequestException(MessageConstant.Brand.NotFoundFailedMessage);

            Origin origin = await _unitOfWork.GetRepository<Origin>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateComponentRequest.OriginId))
                ?? throw new BadHttpRequestException(MessageConstant.Origin.NotFoundFailedMessage);

            component.Category = category;
            component.Brand = brand;
            component.Origin = origin;

            component.Name = string.IsNullOrEmpty(updateComponentRequest.Name) ? component.Name : updateComponentRequest.Name;
            component.Description = string.IsNullOrEmpty(updateComponentRequest.Description) ? component.Description : updateComponentRequest.Description;
            component.SellingPrice = updateComponentRequest.SellingPrice.HasValue ? updateComponentRequest.SellingPrice.Value : component.SellingPrice;
            component.StockPrice = updateComponentRequest.StockPrice.HasValue ? updateComponentRequest.StockPrice.Value : component.StockPrice;
            component.TimeWarranty = updateComponentRequest.TimeWarranty.HasValue ? updateComponentRequest.TimeWarranty.Value : component.TimeWarranty;
            
            if (!updateComponentRequest.Status.HasValue)
            {
                throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
            }
            else
            {
                component.Status = updateComponentRequest.Status?.GetDescriptionFromEnum();
            }
            _unitOfWork.GetRepository<MachineComponent>().UpdateAsync(component);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

    }
}
