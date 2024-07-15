using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class ImageComponent
{
    public Guid Id { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid? MachineComponentId { get; set; }

    public virtual MachineComponent? MachineComponent { get; set; }
}
