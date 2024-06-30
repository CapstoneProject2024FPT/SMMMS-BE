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
using static System.Net.Mime.MediaTypeNames;
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

        //public async Task<Guid> CreateNewMachinerys(CreateNewMachineryRequest createNewMachineryRequest)
        //{
        //    Machinery machinery = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
        //        predicate: x => x.Name.Equals(createNewMachineryRequest.Name));
        //    if (machinery != null) throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNameExisted);
        //    Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
        //        predicate: x => x.Id.Equals(createNewMachineryRequest.CategoryId));
        //    if (category == null) throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
        //    machinery = _mapper.Map<Machinery>(createNewMachineryRequest);

        //    await _unitOfWork.GetRepository<Machinery>().InsertAsync(machinery);
        //    bool isSuccess = await _unitOfWork.CommitAsync() > 0;
        //    if (!isSuccess) throw new BadHttpRequestException(MessageConstant.Machinery.CreateNewMachineryFailedMessage);
        //    return machinery.Id;
        //}
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

            // Create a new Machinery entity
            Machinery newMachinery = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                OriginId = request.OriginId,
                BrandId = request.BrandId,
                Model = request.Model,
                Description = request.Description,
                //SerialNumber = TimeUtils.GetTimestamp(currentTime),
                Status = MachineryStatus.Available.GetDescriptionFromEnum(),
                StockPrice = request.StockPrice,
                SellingPrice = request.SellingPrice,
                Priority = request.Priority,
                TimeWarranty = request.TimeWarranty,
                CategoryId = request.CategoryId,
                CreateDate = DateTime.Now,

            };

            // Create a list for the machinery specifications
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
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Specification.CreateNewSpecificationFailedMessage);

            return newMachinery.Id;
        }



        public async Task<ICollection<GetMachinerySpecificationsRespone>> GetMachineryList(MachineryFilter filter)
        {
            var machineryList = await _unitOfWork.GetRepository<Machinery>()
                .GetListAsync(
                    selector: x => x,
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.Priority),
                    include: x => x.Include(x => x.Inventories)
                                   .Include(x => x.Brand)
                                   .Include(x => x.Origin)
                                   .Include(x => x.Category)
                                   .Include(x => x.ImagesAlls)
                                   .Include(x => x.Specifications))
                ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);

            var machineryResponses = machineryList.Select(machinery => new GetMachinerySpecificationsRespone
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
            }).ToList();

            return machineryResponses;
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
