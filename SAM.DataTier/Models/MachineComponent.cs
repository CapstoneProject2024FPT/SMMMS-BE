﻿using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class MachineComponent
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<MachinePartMachine> MachinePartMachines { get; set; } = new List<MachinePartMachine>();
}
