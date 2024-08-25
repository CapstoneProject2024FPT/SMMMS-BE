using AutoMapper;
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
using SAM.BusinessTier.Enums.EnumStatus;
using Microsoft.Identity.Client;
using System.Linq;
using SAM.BusinessTier.Enums.EnumTypes;
using SAM.BusinessTier.Extensions;
using SAM.BusinessTier.Payload.Favorite;
using SAM.BusinessTier.Payload.Origin;

namespace SAM.BusinessTier.Services.Implements
{
    public class UserService : BaseService<UserService>, IUserService
    {
        readonly private INotificationService _notificationService;
        public UserService(IUnitOfWork<SamDevContext> unitOfWork, ILogger<UserService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, INotificationService notificationService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        //public async Task<bool> AddRankToAccount(Guid id, List<Guid> request)
        //{

        //    Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
        //        predicate: x => x.Id.Equals(id))
        //    ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);

        //    List<Guid> currentRankIds = (List<Guid>)await _unitOfWork.GetRepository<AccountRank>().GetListAsync(
        //        selector: x => x.RankId,
        //        predicate: x => x.AccountId.Equals(id));

        //    (List<Guid> idsToRemove, List<Guid> idsToAdd, List<Guid> idsToKeep) splittedRankIds =
        //        CustomListUtil.splitidstoaddandremove(currentRankIds, request);


        //    if (splittedRankIds.idsToAdd.Count > 0)
        //    {
        //        List<AccountRank> ranksToInsert = new List<AccountRank>();
        //        splittedRankIds.idsToAdd.ForEach(rankId => ranksToInsert.Add(new AccountRank
        //        {
        //            Id = Guid.NewGuid(),
        //            AccountId = id,
        //            RankId = rankId,
        //        }));
        //        await _unitOfWork.GetRepository<AccountRank>().InsertRangeAsync(ranksToInsert);
        //    }

        //    if (splittedRankIds.idsToRemove.Count > 0)
        //    {
        //        List<AccountRank> ranksToDelete = (List<AccountRank>)await _unitOfWork.GetRepository<AccountRank>()
        //            .GetListAsync(predicate: x =>
        //                x.AccountId.Equals(id) &&
        //                splittedRankIds.idsToRemove.Contains(x.RankId));

        //         _unitOfWork.GetRepository<AccountRank>().DeleteRangeAsync(ranksToDelete);
        //    }

        //    bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
        //    return isSuccessful;
        //}

        //public async Task<Guid> AddrankForAccount(Guid accountId, Guid rankId)
        //{
        //    var currentUser = GetUsernameFromJwt();

        //    Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
        //        predicate: x => x.Id.Equals(accountId))
        //    ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);
        //    Rank rank = await _unitOfWork.GetRepository<Rank>().SingleOrDefaultAsync(
        //        predicate: x => x.Id.Equals(rankId))
        //    ?? throw new BadHttpRequestException(MessageConstant.Account.NotFoundFailedMessage);

        //    AccountRank accountRank = new AccountRank()
        //    {
        //        Id = Guid.NewGuid(),
        //        AccountId = accountId,
        //        RankId = rankId,
        //       };
            

        //    await _unitOfWork.GetRepository<AccountRank>().InsertAsync(accountRank);
        //    bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
        //    if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Rank.CreateNewRankFailedMessage);

        //    return accountRank.Id;
        //}

        public async Task<bool> ChangePassword(Guid userId, ChangePasswordRequest changePasswordRequest)
        {
            if (userId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.User.EmptyUserIdMessage);

            Account user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(userId))
                ?? throw new BadHttpRequestException(MessageConstant.User.UserNotFoundMessage);

            if (string.IsNullOrEmpty(changePasswordRequest.CurrentPassword) || !PasswordUtil.VerifyHashedPassword(user.Password, changePasswordRequest.CurrentPassword))
            {
                throw new BadHttpRequestException(MessageConstant.User.CheckPasswordFailed);
            }

            user.Password = PasswordUtil.HashPassword(changePasswordRequest.NewPassword);

            _unitOfWork.GetRepository<Account>().UpdateAsync(user);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<Guid> CreateNewUser(CreateNewUserRequest request)
        {
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(request.Username));
            if (account != null) throw new BadHttpRequestException(MessageConstant.User.UserExisted);
            Account account1 = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Email.Equals(request.Email));
            if (account1 != null) throw new BadHttpRequestException(MessageConstant.User.UserEmailExisted);
            account = new Account()
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = PasswordUtil.HashPassword(request.Password),
                Role = RoleEnum.User.GetDescriptionFromEnum(),
                FullName = request.FullName,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Status = UserStatus.Activate.GetDescriptionFromEnum(),
                Email = request.Email,
                Image = request.Image,

            };


            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.User.CreateFailedMessage);
            return account.Id;
        }
        public async Task<Guid> CreateNewStaff(CreateNewStaffRequest request)
        {
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(request.Username));
            if (account != null) throw new BadHttpRequestException(MessageConstant.User.UserExisted);
            Account account1 = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Email.Equals(request.Email));
            if (account1 != null) throw new BadHttpRequestException(MessageConstant.User.UserEmailExisted);
            account = new Account()
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = PasswordUtil.HashPassword(request.Password),
                Role = request.Role,
                FullName = request.FullName,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Status = UserStatus.Activate.GetDescriptionFromEnum(),
                Email = request.Email,
                Image = request.Image,
                YearsOfExperience = request.YearsOfExperience,

            };


            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.User.CreateFailedMessage);
            return account.Id;
        }
        public async Task<ICollection<StaffTaskStatusResponse>> GetStaffTaskStatusesByRole(DateTime targetDate)
        {
            // Lấy danh sách tất cả nhân viên theo vai trò
            var staffList = await _unitOfWork.GetRepository<Account>()
                .GetListAsync(predicate: a => a.Role.Equals(RoleEnum.Technical.GetDescriptionFromEnum()));

            var staffTaskStatuses = new List<StaffTaskStatusResponse>();

            foreach (var staff in staffList)
            {
                var tasks = await _unitOfWork.GetRepository<TaskManager>()
                    .GetListAsync(predicate: t => t.AccountId == staff.Id);

                var taskCountByStatus = tasks.CountTaskEachStatus();

                var specifiedDateTasks = tasks.Where(t => t.ExcutionDate.HasValue && t.ExcutionDate.Value.Date == targetDate.Date).ToList();

                var specifiedDateTaskCountByStatus = specifiedDateTasks.CountTaskEachStatus();

                staffTaskStatuses.Add(new StaffTaskStatusResponse
                {
                    StaffId = staff.Id,
                    StaffName = staff.FullName,
                    TaskStatusCount = taskCountByStatus,
                    TodayTaskStatusCount = specifiedDateTaskCountByStatus
                });
            }

            return staffTaskStatuses;
        }

        public async Task<IPaginate<GetUsersResponse>> GetAllUsers(UserFilter filter, PagingModel pagingModel)
        {
            IPaginate<GetUsersResponse> response = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
                 selector: x => new GetUsersResponse { 
                     Id = x.Id,
                     Username = x.Username,
                     FullName = x.FullName,
                     Email = x.Email,
                     Gender = x.Gender,
                     PhoneNumber = x.PhoneNumber,
                     Image = x.Image,
                     Point = x.Point,
                     YearsOfExperience = x.YearsOfExperience,
                     Role = EnumUtil.ParseEnum<RoleEnum>(x.Role),
                     Status = EnumUtil.ParseEnum<UserStatus>(x.Status),
                     Rank = new RankResponse
                     {
                         Name = x.Rank.Name,
                         Range = x.Rank.Range,
                         Value = x.Rank.Value
                     }
                 },
                 filter: filter,
                 page: pagingModel.page,
                 size: pagingModel.size,
                 orderBy: x => x.OrderBy(x => x.Username)
            );

            //var responseList = new List<GetUsersResponse>();

            //foreach (var account in response.Items)
            //{
            //    // Lấy thông tin rank cho từng account
            //    AccountRank accountRank = await _unitOfWork.GetRepository<AccountRank>().SingleOrDefaultAsync(
            //        predicate: x => x.AccountId.Equals(account.Id)
            //    );

            //    RankResponse rankResponse = null;

            //    if (accountRank != null)
            //    {
            //        // Lấy thông tin rank từ bảng Rank
            //        var rank = await _unitOfWork.GetRepository<Rank>().SingleOrDefaultAsync(
            //            predicate: x => x.Id.Equals(accountRank.RankId)
            //        );

            //        if (rank != null)
            //        {
            //            rankResponse = new RankResponse
            //            {
            //                Name = rank.Name,
            //                Range = rank.Range,
            //                Value = rank.Value
            //            };
            //        }
            //    }

            //    // Map account sang GetUsersResponse và thêm thông tin rank
            //    var userResponse = _mapper.Map<GetUsersResponse>(account);
            //    userResponse.Rank = rankResponse;

            //    responseList.Add(userResponse);
            //}



            return response;
        }


        public async Task<GetUsersResponse> GetUserById(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException(MessageConstant.User.EmptyUserIdMessage);

            // Retrieve the user or throw an exception if not found
            var user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                selector: x => new GetUsersResponse
                {
                    Id = x.Id,
                    Username = x.Username,
                    FullName = x.FullName,
                    Email = x.Email,
                    Gender = x.Gender,
                    PhoneNumber = x.PhoneNumber,
                    Image = x.Image,
                    Point = x.Point,
                    YearsOfExperience = x.YearsOfExperience,
                    Role = EnumUtil.ParseEnum<RoleEnum>(x.Role),
                    Status = EnumUtil.ParseEnum<UserStatus>(x.Status),
                    Rank = new RankResponse
                    {
                        Name = x.Rank.Name,
                        Range = x.Rank.Range,
                        Value = x.Rank.Value
                    }
                })
                ?? throw new BadHttpRequestException(MessageConstant.User.UserNotFoundMessage);

            return user;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            // Tìm kiếm tài khoản dựa trên username
            Account user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Username.Equals(loginRequest.Username));

            // Kiểm tra nếu tài khoản không tồn tại hoặc mật khẩu không khớp
            if (user == null || !PasswordUtil.VerifyHashedPassword(user.Password, loginRequest.Password))
            {
                return null;
            }

            if (user.Role == RoleEnum.Technical.GetDescriptionFromEnum())
            {
                var processingTasks = await _unitOfWork.GetRepository<TaskManager>().SingleOrDefaultAsync(
                    predicate: x => x.AccountId == user.Id && x.Status == TaskManagerStatus.Process.GetDescriptionFromEnum());

                if (processingTasks != null)
                {
                    var title = "Nhiệm vụ đang xử lý";
                    var body = $"Bạn còn nhiệm vụ chưa hoàn thành. Hãy nhanh chóng đến hoàn thành.";
                    await _notificationService.SendPushNotificationAsync(title, body, user.Id);

                    var newNotification = new Notification
                    {
                        Id = Guid.NewGuid(),
                        Title = title,
                        Message = body,
                        AccountId = user.Id,
                        CreatedDate = DateTime.Now
                    };
                    await _unitOfWork.GetRepository<Notification>().InsertAsync(newNotification);
                }
            }

            // Tạo JWT token cho người dùng
            var tokenModel = JwtUtil.GenerateJwtToken(user);

            // Tạo đối tượng LoginResponse
            var loginResponse = new LoginResponse
            {
                Username = loginRequest.Username,
                FullName = user.FullName,
                Role = EnumUtil.ParseEnum<RoleEnum>(user.Role),
                Status = EnumUtil.ParseEnum<UserStatus>(user.Status),
                Id = user.Id,
                TokenModel = tokenModel,
            };

            return loginResponse;
        }


        public async Task<bool> RemoveUserStatus(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadHttpRequestException(MessageConstant.User.EmptyUserIdMessage);

            // Lấy thông tin tài khoản
            Account user = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id))
                ?? throw new BadHttpRequestException(MessageConstant.User.UserNotFoundMessage);

            // Kiểm tra nhiệm vụ chưa hoàn thành
            var hasUnfinishedTasks = await _unitOfWork.GetRepository<TaskManager>().GetListAsync(
                predicate: t => t.AccountId.Equals(id) && t.Status != TaskManagerStatus.Completed.GetDescriptionFromEnum());

            if (hasUnfinishedTasks != null)
                throw new BadHttpRequestException(MessageConstant.User.TaskCheckCompletedFaild);

            // Cập nhật trạng thái của tài khoản
            user.Status = UserStatus.InActivate.GetDescriptionFromEnum();
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
            var task = await _unitOfWork.GetRepository<TaskManager>().SingleOrDefaultAsync(
                predicate: x => x.AccountId.Equals(user.Id) && x.Status == TaskManagerStatus.Process.GetDescriptionFromEnum());
            if (task != null && updateRequest.Status == UserStatus.InActivate)
            {
                throw new BadHttpRequestException(MessageConstant.TaskManager.UpdateStaffProcessTaskFaildMessage);
            }
            user.FullName = string.IsNullOrEmpty(updateRequest.FullName) ? user.FullName : updateRequest.FullName;
            user.PhoneNumber = string.IsNullOrEmpty(updateRequest.PhoneNumber) ? user.PhoneNumber : updateRequest.PhoneNumber;
            user.Email = string.IsNullOrEmpty(updateRequest.Email) ? user.Email : updateRequest.Email;
            user.YearsOfExperience = updateRequest.YearsOfExperience.HasValue ? updateRequest.YearsOfExperience.Value : user.YearsOfExperience;
            user.Image = string.IsNullOrEmpty(updateRequest.Image) ? user.Image : updateRequest.Image;
            if (!updateRequest.Status.HasValue && !updateRequest.Role.HasValue && !updateRequest.Gender.HasValue)
            {
                throw new BadHttpRequestException(MessageConstant.Status.ExsitingValue);
            }
            else
            {
                user.Role = updateRequest.Role.GetDescriptionFromEnum();
                user.Status = updateRequest.Status.GetDescriptionFromEnum();
                user.Gender = updateRequest.Gender.GetDescriptionFromEnum();
            }
            _unitOfWork.GetRepository<Account>().UpdateAsync(user);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
