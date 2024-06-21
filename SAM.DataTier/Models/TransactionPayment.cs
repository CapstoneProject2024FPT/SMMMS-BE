using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class TransactionPayment
{
    public Guid Id { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public Guid? PaymentId { get; set; }

    public virtual Payment? Payment { get; set; }
}
