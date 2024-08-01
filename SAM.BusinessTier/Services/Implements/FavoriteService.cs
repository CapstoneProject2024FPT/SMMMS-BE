using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.City;
using SAM.BusinessTier.Payload.Favorite;
using SAM.BusinessTier.Payload.Machinery;
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
    public class FavoriteService : BaseService<FavoriteService>, IFavoriteService
    {
        public FavoriteService(IUnitOfWork<SamContext> unitOfWork, ILogger<FavoriteService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {

        }

        public async Task<Guid> CreateNewFavorite(CreateNewFavoriteRequest createNewFavoriteRequest)
        {
            var currentUser = GetUsernameFromJwt();

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser));

            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);

            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            // Check if the favorite already exists for this user and machinery
            var existingFavorite = await _unitOfWork.GetRepository<Favorite>().SingleOrDefaultAsync(
                 predicate: f => f.AccountId == account.Id && f.MachineryId == createNewFavoriteRequest.MachineryId);

            if (existingFavorite != null)
                throw new BadHttpRequestException(MessageConstant.Favorite.FavoriteExistedMessage);

            // Create the new favorite
            Favorite favorite = _mapper.Map<Favorite>(createNewFavoriteRequest);
            favorite.Id = Guid.NewGuid();
            favorite.AccountId = account.Id;
            favorite.MachineryId = createNewFavoriteRequest.MachineryId;

            await _unitOfWork.GetRepository<Favorite>().InsertAsync(favorite);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Favorite.CreateFavoriteFailedMessage);

            return favorite.Id;
        }


        public async Task<GetFavoriteResponse> GetFavoriteById()
        {
            var currentUser = GetUsernameFromJwt();

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser));

            var favorites = await _unitOfWork.GetRepository<Favorite>().GetListAsync(
                predicate: f => f.AccountId.Equals(account.Id),
                include: x => x.Include(f => f.Machinery)
                               .ThenInclude(m => m.Brand)
                               .Include(f => f.Machinery.ImagesAlls)
                               .Include(f => f.Account))
                ?? throw new BadHttpRequestException(MessageConstant.Favorite.NotFoundFailedMessage);

            

            var getFavoriteResponse = new GetFavoriteResponse
            {
                
                AccountId = account.Id,
                Name = account.FullName,
                Machinery = favorites.Select(f => new MachineryFavoriteResponse
                {
                    Id = f.Machinery.Id,
                    Name = f.Machinery.Name,
                    Model = f.Machinery.Model,
                    SellingPrice = f.Machinery.SellingPrice,
                    Brand = f.Machinery.Brand == null ? null : new BrandResponse
                    {
                        Id = f.Machinery.Brand.Id,
                        Name = f.Machinery.Brand.Name,
                        Description = f.Machinery.Brand.Description,
                    },
                    Image = f.Machinery.ImagesAlls.Select(image => new MachineryImagesResponse
                    {
                        Id = image.Id,
                        ImageURL = image.ImageUrl,
                        CreateDate = image.CreateDate
                    }).ToList()
                }).ToList()
            };

            return getFavoriteResponse;
        }






        public async Task<bool> DeleteFavorite(Guid machineryId)
        {
            var currentUser = GetUsernameFromJwt();

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(currentUser));
            // Lấy repository cho bảng Favorite
            var favoriteRepository = _unitOfWork.GetRepository<Favorite>();

            // Tìm mục yêu thích theo accountId và machineryId
            var favorite = await favoriteRepository.SingleOrDefaultAsync(
                predicate: f => f.AccountId == account.Id && f.MachineryId == machineryId);

            if (favorite == null)
            {
                // Nếu không tồn tại, ném lỗi không tìm thấy
                throw new BadHttpRequestException(MessageConstant.Favorite.NotFoundFailedMessage);
            }

            // Xóa mục yêu thích
            favoriteRepository.DeleteAsync(favorite);

            // Cam kết các thay đổi vào cơ sở dữ liệu
            var isSuccessful = await _unitOfWork.CommitAsync() > 0;

            // Trả về kết quả thành công hoặc thất bại
            return isSuccessful;
        }

    }
}
