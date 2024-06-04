﻿using SAM.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.User
{
    public class UpdateUserInforRequest
    {
        public string? Password { get; set; }

        public RoleEnum? Role { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public UserStatus? Status { get; set; }

        public string? Email { get; set; }

        //public int? Rank { get; set; }

        public int? YearsOfExperience { get; set; }


        public UpdateUserInforRequest()
        {
        }
    }

}
