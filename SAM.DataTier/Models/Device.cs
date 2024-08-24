using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Device
{
    public Guid Id { get; set; }

    public string Fcmtoken { get; set; } = null!;

    public string? DeviceType { get; set; }

    public DateTime? LastUpdated { get; set; }

    public Guid? AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
