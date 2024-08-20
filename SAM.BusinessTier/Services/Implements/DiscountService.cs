using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Brand;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.City;
using SAM.BusinessTier.Payload.Discount;
using SAM.BusinessTier.Payload.Districts;
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
    public class DiscountService : BaseService<DiscountService>, IDiscountService
    {
        public DiscountService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<DiscountService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }
        public async Task<bool> AddDiscountToCategories(Guid id, List<Guid> request)
        {

            // Retrieve the account or throw an exception if not found
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
            ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);

            // Retrieve current rank IDs associated with the account
            List<Guid> currentDiscountIds = (List<Guid>)await _unitOfWork.GetRepository<DiscountCategory>().GetListAsync(
                selector: x => x.CategoryId,
                predicate: x => x.DiscountId.Equals(id));

            // Determine the IDs to add, remove, and keep
            (List<Guid> idsToRemove, List<Guid> idsToAdd, List<Guid> idsToKeep) splittedRankIds =
                CustomListUtil.splitidstoaddandremove(currentDiscountIds, request);

            // Add new ranks
            if (splittedRankIds.idsToAdd.Count > 0)
            {
                List<DiscountCategory> discountsToInsert = new List<DiscountCategory>();
                splittedRankIds.idsToAdd.ForEach(categoriesId => discountsToInsert.Add(new DiscountCategory
                {
                    Id = Guid.NewGuid(),
                    DiscountId = id,
                    CategoryId = categoriesId,
                }));
                await _unitOfWork.GetRepository<DiscountCategory>().InsertRangeAsync(discountsToInsert);
            }

            // Remove obsolete ranks
            if (splittedRankIds.idsToRemove.Count > 0)
            {
                List<DiscountCategory> discountsToDelete = (List<DiscountCategory>)await _unitOfWork.GetRepository<DiscountCategory>()
                    .GetListAsync(predicate: x =>
                        x.DiscountId.Equals(id) &&
                        splittedRankIds.idsToRemove.Contains(x.CategoryId));

                _unitOfWork.GetRepository<DiscountCategory>().DeleteRangeAsync(discountsToDelete);
            }

            // Commit the changes to the database
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
        public async Task<Guid> CreateNewDiscounts(CreateNewDiscountRequest createNewDiscountRequest)
        {
            Discount discount = await _unitOfWork.GetRepository<Discount>().SingleOrDefaultAsync(
                predicate: x => x.Name.Equals(createNewDiscountRequest.Name));
            if (discount != null) throw new BadHttpRequestException(MessageConstant.Discount.DiscountNameExisted);
            discount = _mapper.Map<Discount>(createNewDiscountRequest);
            discount.Id = Guid.NewGuid();
            discount.Status = DiscountStatus.InActive.GetDescriptionFromEnum();
            discount.CreateDate = DateTime.Now;


            await _unitOfWork.GetRepository<Discount>().InsertAsync(discount);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Discount.CreateNewDiscountFailedMessage);
            return discount.Id;
        }

        public async Task<GetDiscountResponse> GetDiscountById(Guid id)
        {
            var discount = await _unitOfWork.GetRepository<Discount>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Discount.DiscountNotFoundMessage);
                    
            return _mapper.Map<GetDiscountResponse>(discount);
        }

        public async Task<ICollection<GetDiscountResponse>> GetDiscountList(DiscountFilter filter)
        {
            var reponse = await _unitOfWork.GetRepository<Discount>().GetListAsync(
                selector: x => _mapper.Map<GetDiscountResponse>(x),
                filter: filter);
            // Tạo danh sách response
            var responseList = new List<GetDiscountResponse>();

            foreach (var discount in reponse)
            {
                // Lấy thông tin rank cho từng account
                DiscountCategory discountCategory = await _unitOfWork.GetRepository<DiscountCategory>().SingleOrDefaultAsync(
                    predicate: x => x.DiscountId.Equals(discount.Id)
                );

                CategoriesResponse categoriesResponse = null;

                if (discountCategory != null)
                {
                    // Lấy thông tin rank từ bảng Rank
                    var categories = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                        predicate: x => x.Id.Equals(discountCategory.CategoryId)
                    );

                    if (categories != null)
                    {
                        categoriesResponse = new CategoriesResponse
                        {
                            Name = categories.Name
                        };
                    }
                }

                // Map account sang GetUsersResponse và thêm thông tin rank
                var userResponse = _mapper.Map<GetDiscountResponse>(discount);
                userResponse.Categories = categoriesResponse;

                responseList.Add(userResponse);
            }
            return reponse;
        }

        public async Task<bool> RemoveDiscountStatus(Guid id)
        {
            var discount = await _unitOfWork.GetRepository<Discount>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Discount.DiscountNotFoundMessage);
            discount.Status = DiscountStatus.InActive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Discount>().UpdateAsync(discount);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

        public async Task<bool> UpdateDiscount(Guid id, UpdateDiscountRequest updateDiscountRequest)
        {
            var discount = await _unitOfWork.GetRepository<Discount>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Discount.DiscountNotFoundMessage);
            discount.Name = string.IsNullOrEmpty(updateDiscountRequest.Name) ? discount.Name : updateDiscountRequest.Name;
            discount.Value = (int?)(updateDiscountRequest.Value.HasValue ? updateDiscountRequest.Value.Value : updateDiscountRequest.Value);
            
            if (!updateDiscountRequest.Status.HasValue && !updateDiscountRequest.Type.HasValue)
            {
                throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
            }
            else
            {
                discount.Status = updateDiscountRequest.Status.GetDescriptionFromEnum();
                discount.Type = updateDiscountRequest.Type.GetDescriptionFromEnum();
            }
            _unitOfWork.GetRepository<Discount>().UpdateAsync(discount);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }
    }
}
