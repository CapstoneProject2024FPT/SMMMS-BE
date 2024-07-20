using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class MachineComponent
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Status { get; set; }

    public double? StockPrice { get; set; }

    public double? SellingPrice { get; set; }

    public int? TimeWarranty { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? BrandId { get; set; }

    public Guid? OriginId { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<ImageComponent> ImageComponents { get; set; } = new List<ImageComponent>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<MachineryComponentPart> MachineryComponentParts { get; set; } = new List<MachineryComponentPart>();

    public virtual Origin? Origin { get; set; }
}
