using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class MachinerySpecification
{
    public Guid Id { get; set; }

    public Guid MachineryId { get; set; }

    public Guid SpecificationsId { get; set; }

    public virtual Machinery Machinery { get; set; } = null!;

    public virtual Specification Specifications { get; set; } = null!;
}
