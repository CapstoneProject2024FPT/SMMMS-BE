using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class AccountRank
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid RankId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Rank Rank { get; set; } = null!;
}
