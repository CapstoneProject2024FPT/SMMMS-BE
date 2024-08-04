using AutoMapper;
using Azure.Core;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Category;
using SAM.BusinessTier.Payload.User;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Models;
using SAM.DataTier.Paginate;
using SAM.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAM.BusinessTier.Enums.EnumStatus;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Payload.Machinery;
using SAM.BusinessTier.Enums.Other;
using Microsoft.EntityFrameworkCore;


namespace SAM.BusinessTier.Services.Implements
{
    public class CategoryService : BaseService<CategoryService>, ICategoryService
    {
        public CategoryService(IUnitOfWork<SamContext> unitOfWork, ILogger<CategoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateNewCategory(CreateNewCategoryRequest request)
        {
            _logger.LogInformation($"Start create new category: {request}");

            Category newCategory = _mapper.Map<Category>(request);
            newCategory.Kind = request.Kind.GetDescriptionFromEnum();

            if (request.MasterCategoryId != null)
            {
                var parentCategory = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                    predicate: x => x.Id.Equals(request.MasterCategoryId))
                    ?? throw new BadHttpRequestException(MessageConstant.Category.Parent_NotFoundFailedMessage);
                newCategory.Type = CategoryType.Child.GetDescriptionFromEnum();
            }
            else newCategory.Type = CategoryType.Parent.GetDescriptionFromEnum();


            await _unitOfWork.GetRepository<Category>().InsertAsync(newCategory);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Category.CreateCategoryFailedMessage);

            return newCategory.Id;
        }

        public async Task<ICollection<GetCategoriesResponse>> GetCategories(CategoryFilter filter)
        {
            ICollection<GetCategoriesResponse> respone = await _unitOfWork.GetRepository<Category>().GetListAsync(
               selector: x => _mapper.Map<GetCategoriesResponse>(x),
               filter: filter);
            return respone;

        }

        public async Task<GetCategoriesResponse> GetCategory(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Category.CategoryEmptyMessage);
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);
            return _mapper.Map<GetCategoriesResponse>(category);
        }

        public async Task<bool> RemoveCategoryStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Category.CategoryEmptyMessage);
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Machineries))
                ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);         
            foreach (var item in category.Machineries)
            {
                    item.Status = MachineryStatus.UnAvailable.GetDescriptionFromEnum();
            }
            category.Status = CategoryStatus.Inactive.GetDescriptionFromEnum();
            foreach (var item in category.MachineComponents)
            {
                item.Status = ComponentStatus.InActive.GetDescriptionFromEnum();
            }
            category.Status = CategoryStatus.Inactive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }


        public async Task<bool> UpdateCategory(Guid categoryId, UpdateCategoryRequest request)
        {
            _logger.LogInformation($"Start updating product: {categoryId}");
            Category updateCategory = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(categoryId),
                include: x => x.Include(x => x.Machineries))
                ?? throw new BadHttpRequestException(MessageConstant.Category.NotFoundFailedMessage);

            updateCategory.Name = string.IsNullOrEmpty(request.Name) ? updateCategory.Name : request.Name;
            updateCategory.Description = string.IsNullOrEmpty(request.Description) ? updateCategory.Description : request.Description;
            updateCategory.Status = request.Status.GetDescriptionFromEnum();
            updateCategory.Kind = request.Kind.GetDescriptionFromEnum();
            updateCategory.MasterCategoryId = request.MasterCategoryId == Guid.Empty ? updateCategory.MasterCategoryId : request.MasterCategoryId;


            if (request.MasterCategoryId != null)
            {
                updateCategory.Type = CategoryType.Child.GetDescriptionFromEnum();
            }
            else updateCategory.Type = CategoryType.Parent.GetDescriptionFromEnum();
            switch (request.Status)
            {
                case CategoryStatus.Active:
                    foreach (var item in updateCategory.Machineries)
                    {
                        item.Status = MachineryStatus.Available.GetDescriptionFromEnum();
                    }
                    foreach (var item in updateCategory.MachineComponents)
                    {
                        item.Status = MachineryStatus.Available.GetDescriptionFromEnum();
                    }
                    break;
                case CategoryStatus.Inactive:
                    foreach (var item in updateCategory.Machineries)
                    {
                        item.Status = MachineryStatus.UnAvailable.GetDescriptionFromEnum();
                    }
                    foreach (var item in updateCategory.MachineComponents)
                    {
                        item.Status = ComponentStatus.InActive.GetDescriptionFromEnum();
                    }

                    break;
                default:
                    return !true;
            }
            _unitOfWork.GetRepository<Category>().UpdateAsync(updateCategory);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
