using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class MachinePartMachine
{
    public int? Quantity { get; set; }

    public string? Status { get; set; }

    public double? Price { get; set; }

    public Guid MachineComponentId { get; set; }

    public Guid MachineryId { get; set; }

    public virtual MachineComponent MachineComponent { get; set; } = null!;

    public virtual Machinery Machinery { get; set; } = null!;
}
