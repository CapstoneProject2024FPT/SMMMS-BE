﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Payload.Login;
using SAM.BusinessTier.Payload.User;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Services;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Paginate;
using SAM.DataTier.Repository.Interfaces;
using SAM.DataTier.Models;
using SAM.BusinessTier.Payload.Category;
using Microsoft.IdentityModel.Tokens;
using static SAM.BusinessTier.Constants.ApiEndPointConstant;
using SAM.BusinessTier.Enums.EnumStatus;
using Microsoft.Identity.Client;
using System.Linq;

namespace SAM.BusinessTier.Services.Implements
{
    public class UserService : BaseService<UserService>, IUserService
    {
        public UserService(IUnitOfWork<SamContext> unitOfWork, ILogger<UserService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<bool> AddRankToAccount(Guid accountId, List<Guid> request)
        {
            _logger.LogInformation($"Add Rank to Customer: {accountId}");

            // Retrieve the account or throw an exception if not found
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(accountId))
            ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);

            // Retrieve current rank IDs associated with the account
            List<Guid> currentRankIds = (List<Guid>)await _unitOfWork.GetRepository<AccountRank>().GetListAsync(
                selector: x => x.RankId,
                predicate: x => x.AccountId.Equals(accountId));

            // Determine the IDs to add, remove, and keep
            (List<Guid> idsToRemove, List<Guid> idsToAdd, List<Guid> idsToKeep) splittedRankIds =
                CustomListUtil.splitidstoaddandremove(currentRankIds, request);

            // Add new ranks
            if (splittedRankIds.idsToAdd.Count > 0)
            {
                List<AccountRank> ranksToInsert = new List<AccountRank>();
                splittedRankIds.idsToAdd.ForEach(id => ranksToInsert.Add(new AccountRank
                {
                    Id = Guid.NewGuid(),
                    AccountId = accountId,
                    RankId = id,
                }));
                await _unitOfWork.GetRepository<AccountRank>().InsertRangeAsync(ranksToInsert);
            }

            // Remove obsolete ranks
            if (splittedRankIds.idsToRemove.Count > 0)
            {
                List<AccountRank> ranksToDelete = (List<AccountRank>)await _unitOfWork.GetRepository<AccountRank>()
                    .GetListAsync(predicate: x =>
                        x.AccountId.Equals(accountId) &&
                        splittedRankIds.idsToRemove.Contains((Guid)x.RankId));

                 _unitOfWork.GetRepository<AccountRank>().DeleteRangeAsync(ranksToDelete);
            }

            // Commit the changes to the database
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }


        public async Task<Guid> CreateNewUser(CreateNewUserRequest request)
        {
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(request.Username));
            if (account != null) throw new BadHttpRequestException(MessageConstant.User.UserExisted);
            account = new Account()
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = PasswordUtil.HashPassword(request.Password),
                Role = RoleEnum.User.GetDescriptionFromEnum(),
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Status = UserStatus.Activate.GetDescriptionFromEnum(),
                Email = request.Email

            };


            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.User.CreateFailedMessage);
            return account.Id;
        }

        public async Task<IPaginate<GetUsersResponse>> GetAllUsers(UserFilter filter, PagingModel pagingModel)
        {
            IPaginate<GetUsersResponse> respone = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
               selector: x => _mapper.Map<GetUsersResponse>(x),
               filter: filter,
               page: pagingModel.page,
               size: pagingModel.size,
               orderBy: x => x.OrderBy(x => x.Username));
            return respone;
        }

        public async Task<GetUsersResponse> GetUserById(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.User.EmptyUserIdMessage);
            Account user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.User.UserNotFoundMessage);
            return _mapper.Map<GetUsersResponse>(user);
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            Account user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(loginRequest.Username));
            if (user == null || !PasswordUtil.VerifyHashedPassword(user.Password, loginRequest.Password))
            {
                return null;
            }

            var tokenModel = JwtUtil.GenerateJwtToken(user);
            var loginResponse = new LoginResponse
            {
                Username = loginRequest.Username,
                Role = EnumUtil.ParseEnum<RoleEnum>(user.Role),
                Status = EnumUtil.ParseEnum<UserStatus>(user.Status),
                Id = user.Id,
                TokenModel = tokenModel,


            };
            return loginResponse;
        }

        public async Task<bool> RemoveUserStatus(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.User.EmptyUserIdMessage);
            Account user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.User.UserNotFoundMessage);
            user.Status = UserStatus.InActivate.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Account>().UpdateAsync(user);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        //public async Task<bool> UpdateUserInfor(Guid id, UpdateUserInforRequest updateRequest)
        //{
        //    if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.User.EmptyUserIdMessage);
        //    Account user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
        //        predicate: x => x.Id.Equals(id))
        //        ?? throw new BadHttpRequestException(MessageConstant.User.UserNotFoundMessage);
        //    user.Password = string.IsNullOrEmpty(updateRequest.Password) ? user.Password : PasswordUtil.HashPassword(updateRequest.Password);
        //    user.Role = updateRequest.Role.GetDescriptionFromEnum();
        //    user.Status = updateRequest.Status.GetDescriptionFromEnum();
        //    _unitOfWork.GetRepository<Account>().UpdateAsync(user);
        //    bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
        //    return isSuccessful;
        //}

        public async Task<bool> UpdateUserInfor(Guid id, UpdateUserInforRequest updateRequest)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.User.EmptyUserIdMessage);
            Account user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.User.UserNotFoundMessage);
            user.Password = string.IsNullOrEmpty(updateRequest.Password) ? user.Password : PasswordUtil.HashPassword(updateRequest.Password);
            user.Role = updateRequest.Role.GetDescriptionFromEnum();
            user.Status = updateRequest.Status.GetDescriptionFromEnum();
            user.FullName = string.IsNullOrEmpty(updateRequest.FullName) ? user.FullName : updateRequest.FullName;
            user.PhoneNumber = string.IsNullOrEmpty(updateRequest.PhoneNumber) ? user.PhoneNumber : updateRequest.PhoneNumber;
            user.Address = string.IsNullOrEmpty(updateRequest.Address) ? user.Address : updateRequest.Address;
            user.Email = string.IsNullOrEmpty(updateRequest.Email) ? user.Email : updateRequest.Email;
            user.YearsOfExperience = updateRequest.YearsOfExperience.HasValue ? updateRequest.YearsOfExperience.Value : user.YearsOfExperience;
            _unitOfWork.GetRepository<Account>().UpdateAsync(user);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
