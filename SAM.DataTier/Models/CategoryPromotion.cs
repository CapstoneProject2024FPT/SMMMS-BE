using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class CategoryPromotion
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
