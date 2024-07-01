using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class NewsImage
{
    public Guid Id { get; set; }

    public string? ImgUrl { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid? NewsId { get; set; }

    public virtual News? News { get; set; }
}
