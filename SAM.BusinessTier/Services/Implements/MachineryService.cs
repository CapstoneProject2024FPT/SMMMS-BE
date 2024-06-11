using AutoMapper;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums;
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

            // Create a new Machinery entity
            Machinery newMachinery = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Origin = request.Origin,
                Model = request.Model,
                Description = request.Description,
                Quantity = request.Quantity,
                SerialNumber = TimeUtils.GetTimestamp(currentTime),
                Status = ProductStatus.Active.GetDescriptionFromEnum(),
                StockPrice = request.StockPrice,
                SellingPrice = request.SellingPrice,
                Priority = request.Priority,
                Brand = request.Brand,
                ControlSystem = request.ControlSystem,
                TimeWarranty = request.TimeWarranty,
                CategoryId = request.CategoryId,
                

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
                    Unit = spec.Unit
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
            // Fetch all machinery records that match the filter criteria
            var machineryList = await _unitOfWork.GetRepository<Machinery>()
                .GetListAsync(
                    selector: x => x,
                    filter: filter
                );

            var getMachinerysResponseList = new List<GetMachinerysResponse>();

            foreach (var machinery in machineryList)
            {
                var specifications = await _unitOfWork.GetRepository<Specification>()
                    .GetListAsync(
                        selector: x => new SpecificationsResponse
                        {
                            SpecificationId = x.Id,
                            MachineryId = x.MachineryId,
                            Name = x.Name,
                            Value = (float)x.Value,
                            Unit = x.Unit,
                        },
                        predicate: x => x.MachineryId.Equals(machinery.Id)
                    );

                var category = await _unitOfWork.GetRepository<Category>()
                    .SingleOrDefaultAsync(
                        selector: x => new CategoryResponse
                        {
                            Name = x.Name,
                            Type = EnumUtil.ParseEnum<CategoryType>(x.Type),
                        },
                        predicate: x => x.Id.Equals(machinery.CategoryId)
                    );

                var images = await _unitOfWork.GetRepository<ImagesAll>()
                    .GetListAsync(
                        selector: x => new MachineryImagesResponse
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
                    Origin = machinery.Origin,
                    Model = machinery.Model,
                    Description = machinery.Description,
                    Specifications = specifications.ToList(),
                    SerialNumber = machinery.SerialNumber,
                    SellingPrice = machinery.SellingPrice,
                    Priority = machinery.Priority,
                    Brand = machinery.Brand,
                    ControlSystem = machinery.ControlSystem,
                    TimeWarranty = machinery.TimeWarranty,
                    Status = EnumUtil.ParseEnum<MachineryStatus>(machinery.Status),
                    Category = category,
                    Image = images.ToList(),
                    CreateDate = machinery.CreateDate,
                     
                };

                // Add the response object to the list
                getMachinerysResponseList.Add(getMachinerysResponse);
            }

            // Return the list of response objects
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
                Origin = machinery.Origin,
                Model = machinery.Model,
                Description = machinery.Description,
                Specifications = (List<SpecificationsResponse>)await _unitOfWork.GetRepository<Specification>()
                    .GetListAsync(
                        selector: x => new SpecificationsResponse()
                        {
                            SpecificationId = x.Id,
                            MachineryId = x.MachineryId,
                            Name = x.Name,
                            Value = (float)x.Value,
                            Unit = x.Unit,
                        },
                        predicate: x => x.MachineryId.Equals(id)
                    ),
                SerialNumber = machinery.SerialNumber,
                SellingPrice = machinery.SellingPrice,
                Priority = machinery.Priority,
                Brand = machinery.Brand,
                ControlSystem = machinery.ControlSystem,
                TimeWarranty = machinery.TimeWarranty,
                Status = EnumUtil.ParseEnum<MachineryStatus>(machinery.Status),
                Category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                        selector: x => new CategoryResponse()
                        {
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
                    )
            };

            return getMachinerySpecificationsRespone;
        }

        public async Task<bool> RemoveMachineryStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Machinery.EmptyMachineryIdMessage);
            Machinery product = await _unitOfWork.GetRepository<Machinery>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Machinery.MachineryNotFoundMessage);
            product.Status = ProductStatus.Inactive.GetDescriptionFromEnum();
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
            Category category = (await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateProductRequest.CategoryId))) != null
                ? await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                    predicate: x => x.Id.Equals(updateProductRequest.CategoryId))
                : throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);

            product.Name = string.IsNullOrEmpty(updateProductRequest.Name) ? product.Name : updateProductRequest.Name;
            product.Origin = string.IsNullOrEmpty(updateProductRequest.Origin) ? product.Origin : updateProductRequest.Origin;
            product.Model = string.IsNullOrEmpty(updateProductRequest.Model) ? product.Model : updateProductRequest.Model;
            product.SellingPrice = (updateProductRequest.SellingPrice >= 0) ? product.SellingPrice : updateProductRequest.SellingPrice;
            product.StockPrice = (updateProductRequest.StockPrice >= 0) ? product.StockPrice : updateProductRequest.StockPrice;
            product.Description = string.IsNullOrEmpty(updateProductRequest.Description) ? product.Description : updateProductRequest.Description;
            product.Status = string.IsNullOrEmpty(updateProductRequest.Status.GetDescriptionFromEnum())
                ? updateProductRequest.Status.ToString()
                : updateProductRequest.Status.GetDescriptionFromEnum();
            
            product.Priority = (updateProductRequest.Priority >= 0) ? product.Priority : updateProductRequest.Priority;
            _unitOfWork.GetRepository<Machinery>().UpdateAsync(product);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }


    }
}
