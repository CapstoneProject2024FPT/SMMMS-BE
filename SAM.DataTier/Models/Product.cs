using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Origin { get; set; }

    public int? Quantity { get; set; }

    public string? Status { get; set; }

    public DateTime? ExpMaintanceDate { get; set; }

    public string? SerialNumber { get; set; }

    public double? StockPrice { get; set; }

    public double? SellingPrice { get; set; }

    public string? ImageUrl { get; set; }

    public Guid? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
