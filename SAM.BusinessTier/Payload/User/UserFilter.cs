using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.User
{
    public class UserFilter
    {
        public string? Username { get; set; }
        public RoleEnum? Role { get; set; }
        public UserStatus? Status { get; set; }
    }
}
