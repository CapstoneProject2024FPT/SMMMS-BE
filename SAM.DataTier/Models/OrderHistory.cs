using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class OrderHistory
{
    public Guid Id { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? UserId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual User? User { get; set; }
}
