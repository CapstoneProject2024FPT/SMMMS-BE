using System;
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

    public string? Status { get; set; }

    public string? Email { get; set; }

    public int? Point { get; set; }

    public int? YearsOfExperience { get; set; }

    public string? FcmToken { get; set; }

    public virtual ICollection<AccountRank> AccountRanks { get; set; } = new List<AccountRank>();

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<TaskManager> TaskManagers { get; set; } = new List<TaskManager>();

    public virtual ICollection<WarrantyDetail> WarrantyDetails { get; set; } = new List<WarrantyDetail>();
}
