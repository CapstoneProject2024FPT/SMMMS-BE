using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class MaintenanceDetail
{
    public Guid Id { get; set; }

    public string? TypeMaintenance { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public Guid? MaintenanceId { get; set; }

    public Guid? ContractId { get; set; }

    public virtual ProductContract? Contract { get; set; }

    public virtual Maintenance? Maintenance { get; set; }
}
