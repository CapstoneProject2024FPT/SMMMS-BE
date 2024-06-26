﻿using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Image { get; set; }

    public string? Gender { get; set; }

    public string? Role { get; set; }

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Status { get; set; }

    public string? Email { get; set; }

    public double? Amount { get; set; }

    public int? YearsOfExperience { get; set; }

    public virtual ICollection<AccountRank> AccountRanks { get; set; } = new List<AccountRank>();

    public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<WarrantyDetail> WarrantyDetails { get; set; } = new List<WarrantyDetail>();
}
