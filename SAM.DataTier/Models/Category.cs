using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public Guid? MasterCategoryId { get; set; }

    public virtual ICollection<Category> InverseMasterCategory { get; set; } = new List<Category>();

    public virtual Category? MasterCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
