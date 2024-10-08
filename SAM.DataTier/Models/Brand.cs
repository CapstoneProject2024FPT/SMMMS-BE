﻿using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Brand
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Status { get; set; }

    public string? Urlimage { get; set; }

    public virtual ICollection<MachineComponent> MachineComponents { get; set; } = new List<MachineComponent>();

    public virtual ICollection<Machinery> Machineries { get; set; } = new List<Machinery>();
}
