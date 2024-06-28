using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Login;
using SAM.BusinessTier.Payload.User;
using SAM.DataTier.Paginate;


namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<Guid> CreateNewUser(CreateNewUserRequest request);
        Task<IPaginate<GetUsersResponse>> GetAllUsers(UserFilter filter, PagingModel pagingModel);
        Task<GetUsersResponse> GetUserById(Guid id);
        Task<bool> UpdateUserInfor(Guid id, UpdateUserInforRequest updateRequest);
        Task<bool> RemoveUserStatus(Guid id);

        Task<bool> AddRankToAccount(Guid id, List<Guid> request);
    }
}
