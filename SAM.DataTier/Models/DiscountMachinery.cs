using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class DiscountMachinery
{
    public Guid Id { get; set; }

    public Guid? MachineryId { get; set; }

    public Guid? DiscountId { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual Machinery? Machinery { get; set; }
}
