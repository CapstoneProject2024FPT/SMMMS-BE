using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class City
{
    public Guid Id { get; set; }

    public int? UnitId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Slug { get; set; }

    public string? Status { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public string? NameEn { get; set; }
}
