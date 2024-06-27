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
                    predicate: c => c.Id == request.OriginId.Value);
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




        public async Task<GetMachinerysResponse> GetMachineryById(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Machinery.EmptyMachineryIdMessage);
            Machinery product = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
            return _mapper.Map<GetMachinerysResponse>(product);
        }

       
        public async Task<ICollection<GetMachinerysResponse>> GetMachineryList(MachineryFilter filter)
        {
            var machineryList = await _unitOfWork.GetRepository<Machinery>()
                .GetListAsync(
                    selector: x => x,
                    filter: filter,
                    orderBy: x => x.OrderBy(x => x.Priority)
                );

            var getMachinerysResponseList = new List<GetMachinerysResponse>();

            foreach (var machinery in machineryList)
            {
                var specifications = await _unitOfWork.GetRepository<Specification>()
                    .GetListAsync(
                        selector: x => new SpecificationsAllResponse
                        {
                            SpecificationId = x.Id,
                            MachineryId = x.MachineryId,
                            Name = x.Name,
                            Value = x.Value,
                        },
                        predicate: x => x.MachineryId.Equals(machinery.Id)
                    );

                var category = await _unitOfWork.GetRepository<Category>()
                    .SingleOrDefaultAsync(
                        selector: x => new CategoryAllResponse
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Type = EnumUtil.ParseEnum<CategoryType>(x.Type),
                        },
                        predicate: x => x.Id.Equals(machinery.CategoryId)
                    );
                var origin = await _unitOfWork.GetRepository<Origin>()
                    .SingleOrDefaultAsync(
                        selector: x => new OriginAllResponse
                        {
                            Id = x.Id,
                            Name = x.Name,
                        },
                        predicate: x => x.Id.Equals(machinery.OriginId)
                    );
                var brand = await _unitOfWork.GetRepository<Brand>()
                    .SingleOrDefaultAsync(
                        selector: x => new BrandResponse
                        {
                            Id = x.Id,
                            Name = x.Name,
                        },
                        predicate: x => x.Id.Equals(machinery.BrandId)
                    );

                var images = await _unitOfWork.GetRepository<ImagesAll>()
                    .GetListAsync(
                        selector: x => new MachineryImagesAllResponse
                        {
                            ImageURL = x.ImageUrl,
                            CreateDate = x.CreateDate,
                        },
                        predicate: x => x.MachineryId.Equals(machinery.Id)
                    );

                var getMachinerysResponse = new GetMachinerysResponse
                {
                    Id = machinery.Id,
                    Name = machinery.Name,
                    Model = machinery.Model,
                    Description = machinery.Description,
                    Specifications = specifications.ToList(),
                    SerialNumber = machinery.SerialNumber,
                    SellingPrice = machinery.SellingPrice,
                    Priority = machinery.Priority,
                    TimeWarranty = machinery.TimeWarranty,
                    Status = EnumUtil.ParseEnum<MachineryStatus>(machinery.Status),
                    Category = category,
                    Image = images.ToList(),
                    CreateDate = machinery.CreateDate
                };

                getMachinerysResponseList.Add(getMachinerysResponse);
            }

            return getMachinerysResponseList;
        }



    public async Task<ICollection<GetMachinerysResponse>> GetMachineryListNotIPaginate(MachineryFilter filter)
        {
            ICollection<GetMachinerysResponse> respone = await _unitOfWork.GetRepository<Machinery>().GetListAsync(
               selector: x => _mapper.Map<GetMachinerysResponse>(x),
               filter: filter
/*               orderBy: x => x.OrderBy(x => x.Priority)*/);
            return respone;
        }

        public async Task<GetMachinerySpecificationsRespone> GetMachinerySpecificationsDetail(Guid id)
        {

            var machinery = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);

            var getMachinerySpecificationsRespone = new GetMachinerySpecificationsRespone
            {
                Name = machinery.Name,
                Brand = await _unitOfWork.GetRepository<Brand>().SingleOrDefaultAsync(
                        selector: x => new BrandResponse()
                        {
                            Id = x.Id,
                            Name = x.Name,
                        },
                        predicate: x => x.Id.Equals(machinery.BrandId)
                    ),
                Model = machinery.Model,
                Description = machinery.Description,
                SerialNumber = machinery.SerialNumber,
                SellingPrice = machinery.SellingPrice,
                Priority = machinery.Priority,
                TimeWarranty = machinery.TimeWarranty,
                Status = EnumUtil.ParseEnum<MachineryStatus>(machinery.Status),
                Origin = await _unitOfWork.GetRepository<Origin>().SingleOrDefaultAsync(
                        selector: x => new OriginResponse()
                        {
                            Id = x.Id,
                            Name = x.Name,
                        },
                        predicate: x => x.Id.Equals(machinery.OriginId)
                    ),
                Category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                        selector: x => new CategoryResponse()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Type = EnumUtil.ParseEnum<CategoryType>(x.Type),
                        },
                        predicate: x => x.Id.Equals(machinery.CategoryId)
                    ),
                Image = (List<MachineryImagesResponse>)await _unitOfWork.GetRepository<ImagesAll>()
                    .GetListAsync(
                        selector: x => new MachineryImagesResponse()
                        {
                            ImageURL = x.ImageUrl,
                            CreateDate = x.CreateDate,
                        },
                        predicate: x => x.MachineryId.Equals(id)
                    ),
                Specifications = (List<SpecificationsResponse>)await _unitOfWork.GetRepository<Specification>()
                    .GetListAsync(
                        selector: x => new SpecificationsResponse()
                        {
                            SpecificationId = x.Id,
                            MachineryId = x.MachineryId,
                            Name = x.Name,
                            Value = x.Value,
                        },
                        predicate: x => x.MachineryId.Equals(id)
                    ),
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

            product.Name = string.IsNullOrEmpty(updateProductRequest.Name) ? product.Name : updateProductRequest.Name;
            //product.Origin = string.IsNullOrEmpty(updateProductRequest.Origin) ? product.Origin : updateProductRequest.Origin;
            product.Model = string.IsNullOrEmpty(updateProductRequest.Model) ? product.Model : updateProductRequest.Model;
            product.SellingPrice = updateProductRequest.SellingPrice.HasValue ? updateProductRequest.SellingPrice.Value : product.SellingPrice;
            product.StockPrice = updateProductRequest.StockPrice.HasValue ? updateProductRequest.StockPrice.Value : product.StockPrice;
            product.Description = string.IsNullOrEmpty(updateProductRequest.Description) ? product.Description : updateProductRequest.Description;
            //product.Brand = string.IsNullOrEmpty(updateProductRequest.Brand) ? product.Brand : updateProductRequest.Brand;
            product.TimeWarranty = updateProductRequest.TimeWarranty.HasValue ? updateProductRequest.TimeWarranty.Value : product.TimeWarranty;
            product.Status = updateProductRequest.Status.GetDescriptionFromEnum();
            //product.Quantity = updateProductRequest.Quantity.HasValue ? updateProductRequest.Quantity.Value : product.Quantity;

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
