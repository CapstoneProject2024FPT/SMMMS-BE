using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Inventory
{
    public Guid Id { get; set; }

    public string? SerialNumber { get; set; }

    public Guid? MachineryId { get; set; }

    public string? Status { get; set; }

    public string? Type { get; set; }

    public virtual Machinery? Machinery { get; set; }

    public virtual ICollection<Warranty> Warranties { get; set; } = new List<Warranty>();
}
