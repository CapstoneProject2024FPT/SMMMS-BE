﻿using SAM.BusinessTier.Enums;
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
        public string? Username { get; set; }

        public RoleEnum? Role { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public UserStatus? Status { get; set; }

        public string? Email { get; set; }

        //public int? Rank { get; set; }

        public int? YearsOfExperience { get; set; }

        public GetUsersResponse(Guid id, string? username, RoleEnum? role, string? fullName, string? phoneNumber, string? address, UserStatus? status, string? email, int? yearsOfExperience)
        {
            Id = id;
            Username = username;
            Role = role;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Address = address;
            Status = status;
            Email = email;
            YearsOfExperience = yearsOfExperience;
        }

        public GetUsersResponse(Guid id, string? username, RoleEnum? role, string? fullName, string? phoneNumber, string? address, UserStatus? status, string? email)
        {
            Id = id;
            Username = username;
            Role = role;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Address = address;
            Status = status;
            Email = email;
        }
    }
}
