using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class WarrantyNote
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid? WarrantyDetailId { get; set; }

    public string? Image { get; set; }

    public virtual WarrantyDetail? WarrantyDetail { get; set; }
}
