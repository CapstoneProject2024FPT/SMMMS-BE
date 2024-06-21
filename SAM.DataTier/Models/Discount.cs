using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Discount
{
    public Guid Id { get; set; }

    public string? Type { get; set; }

    public double? Value { get; set; }

    public virtual ICollection<DiscountMachinery> DiscountMachineries { get; set; } = new List<DiscountMachinery>();
}
