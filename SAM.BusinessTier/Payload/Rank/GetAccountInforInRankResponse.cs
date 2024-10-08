﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Rank
{
    public class GetAccountInforInRankResponse
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }

        public string? Image { get; set; }

        public string? Role { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? Status { get; set; }

        public string? Email { get; set; }

        public double? Point { get; set; }
    }
}
