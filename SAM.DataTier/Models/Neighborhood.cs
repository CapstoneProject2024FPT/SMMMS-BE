using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Neighborhood
{
    public Guid Id { get; set; }

    public int? UntiId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public string? NameEn { get; set; }

    public Guid? WardsId { get; set; }
}
