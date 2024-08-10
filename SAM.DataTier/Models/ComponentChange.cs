using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class ComponentChange
{
    public Guid Id { get; set; }

    public Guid? WarrantyDetailId { get; set; }

    public Guid? MachineComponentId { get; set; }

    public string? Image { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual MachineComponent? MachineComponent { get; set; }

    public virtual WarrantyDetail? WarrantyDetail { get; set; }
}
