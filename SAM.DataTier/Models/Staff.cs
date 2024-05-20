using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Staff
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();

    public virtual User? User { get; set; }
}
