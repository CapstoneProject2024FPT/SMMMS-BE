using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class OrderDetail
{
    public Guid Id { get; set; }

    public int? Quantity { get; set; }

    public double? SellingPrice { get; set; }

    public double? TotalAmount { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? MachineryId { get; set; }

    public virtual Machinery? Machinery { get; set; }

    public virtual Order? Order { get; set; }
}
