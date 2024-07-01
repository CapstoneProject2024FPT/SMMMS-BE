using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class NewsCategory
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();
}
