using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Specification
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Value { get; set; }

    public Guid? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<MachinerySpecification> MachinerySpecifications { get; set; } = new List<MachinerySpecification>();
}
