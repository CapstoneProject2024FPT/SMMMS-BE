using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class OrderDetail
{
    public Guid Id { get; set; }

    public int? Quantity { get; set; }

    public double? SellingPrice { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? ProductId { get; set; }

    public double? TotalAmount { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<OrderDetailHistoy> OrderDetailHistoys { get; set; } = new List<OrderDetailHistoy>();

    public virtual Product? Product { get; set; }

    public virtual ICollection<ProductContract> ProductContracts { get; set; } = new List<ProductContract>();
}
