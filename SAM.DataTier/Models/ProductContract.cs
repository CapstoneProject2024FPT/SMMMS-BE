using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class ProductContract
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public Guid? OrderDetailId { get; set; }

    public virtual ICollection<MaintenanceDetail> MaintenanceDetails { get; set; } = new List<MaintenanceDetail>();

    public virtual OrderDetail? OrderDetail { get; set; }
}
