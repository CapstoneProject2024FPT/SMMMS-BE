using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class WarrantyDetail
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

    public Guid? AccountId { get; set; }

    public Guid WarrantyId { get; set; }

    public Guid? AddressId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<ComponentChange> ComponentChanges { get; set; } = new List<ComponentChange>();

    public virtual ICollection<TaskManager> TaskManagers { get; set; } = new List<TaskManager>();

    public virtual Warranty Warranty { get; set; } = null!;

    public virtual ICollection<WarrantyNote> WarrantyNotes { get; set; } = new List<WarrantyNote>();
}
