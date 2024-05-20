using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public Guid? UserId { get; set; }

    public virtual User? User { get; set; }
}
