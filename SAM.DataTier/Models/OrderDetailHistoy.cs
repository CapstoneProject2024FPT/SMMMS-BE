using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class OrderDetailHistoy
{
    public Guid Id { get; set; }

    public int? Quatity { get; set; }

    public double? SellingPrice { get; set; }

    public Guid? OrderDetailId { get; set; }

    public virtual OrderDetail? OrderDetail { get; set; }
}
