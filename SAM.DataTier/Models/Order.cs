﻿using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public string? InvoiceCode { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? Type { get; set; }

    public double? FinalAmount { get; set; }

    public double? TotalAmount { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? AddressId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<TaskManager> TaskManagers { get; set; } = new List<TaskManager>();
}
