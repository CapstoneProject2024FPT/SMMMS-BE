using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Area
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
