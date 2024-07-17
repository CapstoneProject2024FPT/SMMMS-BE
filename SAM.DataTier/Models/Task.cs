using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Task
{
    public Guid Id { get; set; }

    public string? Type { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public Guid? WarrantyDetailId { get; set; }

    public Guid? OrderId { get; set; }
}
