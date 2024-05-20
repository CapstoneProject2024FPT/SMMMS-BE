using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Payment
{
    public Guid Id { get; set; }

    public string? Method { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
