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

    public string? Status { get; set; }

    public string? Type { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid? NewsCategoryId { get; set; }

    public Guid? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual NewsCategory? NewsCategory { get; set; }

    public virtual ICollection<NewsImage> NewsImages { get; set; } = new List<NewsImage>();
}
