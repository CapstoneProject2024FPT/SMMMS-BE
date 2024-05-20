using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Customer
{
    public Guid Id { get; set; }

    public Guid? RankId { get; set; }

    public Guid? UserIdd { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Rank? Rank { get; set; }

    public virtual User? UserIddNavigation { get; set; }
}
