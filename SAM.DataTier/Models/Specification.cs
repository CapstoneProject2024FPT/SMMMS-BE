using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Specification
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public double? Value { get; set; }

    public Guid? MachineryId { get; set; }

    public virtual Machinery? Machinery { get; set; }
}
