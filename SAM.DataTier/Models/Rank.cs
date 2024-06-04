using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Rank
{
    public Guid Id { get; set; }

    public string? RankName { get; set; }

    public int? Range { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
