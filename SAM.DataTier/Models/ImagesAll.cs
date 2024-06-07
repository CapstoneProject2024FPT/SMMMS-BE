using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class ImagesAll
{
    public Guid Id { get; set; }

    public string? ImageUrl { get; set; }

    public Guid? MachineryId { get; set; }

    public virtual Machinery? Machinery { get; set; }
}
