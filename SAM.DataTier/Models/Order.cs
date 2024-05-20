using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public string? InvoiceCode { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public double? TotalAmount { get; set; }

    public double? FinalAmount { get; set; }

    public Guid? PaymentId { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? PromotionId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    public virtual Payment? Payment { get; set; }

    public virtual Promotion? Promotion { get; set; }
}
