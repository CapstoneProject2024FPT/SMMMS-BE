using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Maintenance
{
    public Guid Id { get; set; }

    public string? Type { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? CompletionDate { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public string? Comments { get; set; }

    public DateTime? NextMaintenanceDate { get; set; }

    public int? Priority { get; set; }

    public Guid? TechnicalId { get; set; }

    public Guid? ComponentParts { get; set; }

    public Guid? OrderDetailId { get; set; }
}
