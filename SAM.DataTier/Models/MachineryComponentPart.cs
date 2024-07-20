using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class MachineryComponentPart
{
    public Guid Id { get; set; }

    public Guid? MachineryId { get; set; }

    public Guid MachineComponentsId { get; set; }

    public virtual MachineComponent MachineComponents { get; set; } = null!;

    public virtual Machinery? Machinery { get; set; }
}
