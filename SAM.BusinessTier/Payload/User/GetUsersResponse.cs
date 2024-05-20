using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.User
{
    public class GetUsersResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public UserStatus Status { get; set; }
        public RoleEnum Role { get; set; }


        public GetUsersResponse(Guid id, string username, UserStatus status, RoleEnum role)
        {
            Id = id;
            Username = username;
            Status = status;
            Role = role;
        }

        public GetUsersResponse()
        {
        }
    }
}
