using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class SerialNumberTracker
{
    public string MachineryCode { get; set; } = null!;

    public int? CurrentSerialNumber { get; set; }

    public virtual ICollection<Machinery> Machineries { get; set; } = new List<Machinery>();
}
