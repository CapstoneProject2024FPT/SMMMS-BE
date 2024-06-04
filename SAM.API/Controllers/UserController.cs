using DentalLabManagement.API.Controllers;
using SAM.BusinessTier.Constants;
using SAM.BusinessTier.Enums;
using SAM.BusinessTier.Error;
using SAM.BusinessTier.Payload;
using SAM.BusinessTier.Payload.Login;
using SAM.BusinessTier.Payload.User;
using SAM.BusinessTier.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SAM.API.Controllers
{
    [ApiController]
    public class UserController : BaseController<UserController>
    {
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService) : base(logger)
        {
            _userService = userService;
        }
        [HttpPost(ApiEndPointConstant.Authentication.Login)]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var response = await _userService.Login(loginRequest);
            if (response == null)
            {
                return Unauthorized(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Error = MessageConstant.LoginMessage.InvalidUsernameOrPassword,
                    TimeStamp = DateTime.Now
                });
            }
            if (response.Status == UserStatus.Deactivate)
            {
                return Unauthorized(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Error = MessageConstant.LoginMessage.DeactivatedAccount,
                    TimeStamp = DateTime.Now
                });
            }
            return Ok(response);
        }
        //[CustomAuthorize(RoleEnum.Admin, RoleEnum.User)]
        [HttpPost(ApiEndPointConstant.User.UsersEndPoint)]
        public async Task<IActionResult> CreateNewUser(CreateNewUserRequest createNewUserRequest)
        {
            var response = await _userService.CreateNewUser(createNewUserRequest);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.User.UsersEndPoint)]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserFilter filter, [FromQuery] PagingModel pagingModel)
        {
            var response = await _userService.GetAllUsers(filter, pagingModel);
            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.User.UserEndPoint)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var response = await _userService.GetUserById(id);
            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.User.UserEndPoint)]
        public async Task<IActionResult> UpdateUserInfor(Guid id, UpdateUserInforRequest updateRequest)
        {
            var isSuccessful = await _userService.UpdateUserInfor(id, updateRequest);
            if (!isSuccessful) return Ok(MessageConstant.User.UpdateStatusFailedMessage);
            return Ok(MessageConstant.User.UpdateStatusSuccessMessage);
        }
        [HttpDelete(ApiEndPointConstant.User.UserEndPoint)]
        public async Task<IActionResult> RemoveUserStatus(Guid id)
        {
            var isSuccessful = await _userService.RemoveUserStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.User.UpdateStatusFailedMessage);
            return Ok(MessageConstant.User.UpdateStatusSuccessMessage);
        }
    }
}
