using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Origin
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Machinery> Machineries { get; set; } = new List<Machinery>();
}
