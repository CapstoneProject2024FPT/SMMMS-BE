using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Maintenance
{
    public Guid Id { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EstimatedTime { get; set; }

    public DateTime? CompletedTime { get; set; }

    public string? Status { get; set; }

    public Guid? StaffId { get; set; }

    public virtual ICollection<MaintenanceDetail> MaintenanceDetails { get; set; } = new List<MaintenanceDetail>();

    public virtual Staff? Staff { get; set; }
}
