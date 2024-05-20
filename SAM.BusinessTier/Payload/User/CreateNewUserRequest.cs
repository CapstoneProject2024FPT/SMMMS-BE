using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.User
{
    public class CreateNewUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public RoleEnum Role { get; set; }
        public StaffInforRequest Staff { get; set; }
        public ManagerInforRequest Manager { get; set; }

    }
    public class StaffInforRequest
    {
        public string? FullName { get; set; }

    }
    public class ManagerInforRequest
    {
        public string? FullName { get; set; }

    }
}
