using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Discount
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public double? Value { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<DiscountCategory> DiscountCategories { get; set; } = new List<DiscountCategory>();
}
