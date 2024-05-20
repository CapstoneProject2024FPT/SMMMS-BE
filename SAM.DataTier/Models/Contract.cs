using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Contract
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? OrderDetailId { get; set; }
}
