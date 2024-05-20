using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Blog
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public Guid? StaffId { get; set; }

    public string? Status { get; set; }

    public virtual Staff? Staff { get; set; }
}
