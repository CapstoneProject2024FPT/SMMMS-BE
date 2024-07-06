using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Ward
{
    public Guid Id { get; set; }

    public int? UnitId { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public Guid? DistrictId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual District? District { get; set; }
}
