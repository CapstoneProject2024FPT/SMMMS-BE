using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string? FullName { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Status { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual Staff IdNavigation { get; set; } = null!;

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();
}
