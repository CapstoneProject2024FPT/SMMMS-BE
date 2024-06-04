using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public Guid? MasterCategoryId { get; set; }

    public virtual ICollection<Category> InverseMasterCategory { get; set; } = new List<Category>();

    public virtual ICollection<Machinery> Machineries { get; set; } = new List<Machinery>();

    public virtual Category? MasterCategory { get; set; }
}
