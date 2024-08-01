using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Favorite
{
    public Guid Id { get; set; }

    public Guid MachineryId { get; set; }

    public Guid? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Machinery Machinery { get; set; } = null!;
}
