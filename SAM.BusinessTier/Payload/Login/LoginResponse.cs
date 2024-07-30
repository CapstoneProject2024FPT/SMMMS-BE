using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Login
{
    public class LoginResponse
    {
        public TokenModel TokenModel { get; set; }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public UserStatus Status { get; set; }
        public RoleEnum Role { get; set; }

        public LoginResponse()
        {
        }

        public LoginResponse(TokenModel tokenModel, Guid id, string username, string password, string fullName, UserStatus status, RoleEnum role)
        {
            TokenModel = tokenModel;
            Id = id;
            Username = username;
            Password = password;
            FullName = fullName;
            Status = status;
            Role = role;
        }
    }

}
