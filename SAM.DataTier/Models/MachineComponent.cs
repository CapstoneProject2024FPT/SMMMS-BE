using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class MachineComponent
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
