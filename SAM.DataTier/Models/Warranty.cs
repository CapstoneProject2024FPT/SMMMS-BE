using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Warranty
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

    public Guid? InventoryId { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? AddressId { get; set; }

    public virtual Inventory? Inventory { get; set; }

    public virtual ICollection<WarrantyDetail> WarrantyDetails { get; set; } = new List<WarrantyDetail>();
}
