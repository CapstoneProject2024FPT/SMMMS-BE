using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public string? InvoiceCode { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public double? FinalAmount { get; set; }

    public double? TotalAmount { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? AreaId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Area? Area { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
