﻿using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Payment
{
    public Guid Id { get; set; }

    public double? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public Guid? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<TransactionPayment> TransactionPayments { get; set; } = new List<TransactionPayment>();
}
