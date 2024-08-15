using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Rank
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public int? Range { get; set; }

    public int? Value { get; set; }

    public virtual ICollection<AccountRank> AccountRanks { get; set; } = new List<AccountRank>();
}
