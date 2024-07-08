using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Machinery
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Model { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public double? StockPrice { get; set; }

    public double? SellingPrice { get; set; }

    public int? Priority { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? TimeWarranty { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? OriginId { get; set; }

    public Guid? BrandId { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }

    public virtual SerialNumberTracker? CodeNavigation { get; set; }

    public virtual ICollection<DiscountMachinery> DiscountMachineries { get; set; } = new List<DiscountMachinery>();

    public virtual ICollection<ImagesAll> ImagesAlls { get; set; } = new List<ImagesAll>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<MachinePartMachine> MachinePartMachines { get; set; } = new List<MachinePartMachine>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Origin? Origin { get; set; }

    public virtual ICollection<Specification> Specifications { get; set; } = new List<Specification>();
}
