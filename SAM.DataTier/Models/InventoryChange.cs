using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class InventoryChange
{
    public Guid Id { get; set; }

    public Guid? WarrantyDetailId { get; set; }

    public Guid? NewInventoryId { get; set; }

    public Guid? OldInventoryId { get; set; }

    public string? Image { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual WarrantyDetail? WarrantyDetail { get; set; }
}
