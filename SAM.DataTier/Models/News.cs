using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class News
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? NewsContent { get; set; }

    public string? Cover { get; set; }

    public Guid? MachineryId { get; set; }

    public virtual Machinery? Machinery { get; set; }

    public virtual ICollection<NewsImage> NewsImages { get; set; } = new List<NewsImage>();
}
