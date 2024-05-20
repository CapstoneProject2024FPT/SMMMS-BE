using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Rank
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public int? Range { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
