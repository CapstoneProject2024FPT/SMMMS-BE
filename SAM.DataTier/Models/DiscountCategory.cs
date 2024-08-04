using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class DiscountCategory
{
    public Guid Id { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid DiscountId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Discount Discount { get; set; } = null!;
}
