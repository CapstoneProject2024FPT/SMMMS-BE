using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Promotion
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public Guid? CategoryPromotionId { get; set; }

    public virtual CategoryPromotion? CategoryPromotion { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
