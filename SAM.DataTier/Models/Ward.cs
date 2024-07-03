using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Ward
{
    public Guid Id { get; set; }

    public int? UnitId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public string? NameEn { get; set; }

    public Guid? DistrictsId { get; set; }

    public virtual District? Districts { get; set; }

    public virtual ICollection<Neighborhood> Neighborhoods { get; set; } = new List<Neighborhood>();
}
