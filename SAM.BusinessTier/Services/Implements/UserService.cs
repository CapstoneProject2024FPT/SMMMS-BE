using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums;
using SAM.BusinessTier.Payload.Login;
using SAM.BusinessTier.Payload.User;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Services.Interfaces;
using SAM.BusinessTier.Services;
using SAM.BusinessTier.Utils;
using SAM.DataTier.Paginate;
using SAM.DataTier.Repository.Interfaces;
using SAM.DataTier.Models;

namespace SAM.BusinessTier.Services.Implements
{
    public class UserService : BaseService<UserService>, IUserService
    {
        public UserService(IUnitOfWork<SamContext> unitOfWork, ILogger<UserService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
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
                Email = request.Email,
                Amount = request.Amount,
                YearsOfExperience = request.YearsOfExperience,

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
            user.Status = UserStatus.Deactivate.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Account>().UpdateAsync(user);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateUserInfor(Guid id, UpdateUserInforRequest updateRequest)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.User.EmptyUserIdMessage);
            Account user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.User.UserNotFoundMessage);
            user.Password = string.IsNullOrEmpty(updateRequest.Password) ? user.Password : PasswordUtil.HashPassword(updateRequest.Password);
            user.Role = updateRequest.Role.GetDescriptionFromEnum();
            user.Status = updateRequest.Status.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<Account>().UpdateAsync(user);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
