using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class TransactionPayment
{
    public Guid Id { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public string? InvoiceId { get; set; }

    public double? TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? PayType { get; set; }

    public string? TransactionJson { get; set; }

    public Guid? PaymentId { get; set; }

    public virtual Payment? Payment { get; set; }
}
