using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Inventory
{
    public Guid Id { get; set; }

    public string? SerialNumber { get; set; }

    public string? Status { get; set; }

    public string? Type { get; set; }

    public string? Condition { get; set; }

    public string? IsRepaired { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? SoldDate { get; set; }

    public Guid? MachineryId { get; set; }

    public Guid? MachineComponentsId { get; set; }

    public Guid? MasterInventoryId { get; set; }

    public virtual ICollection<Inventory> InverseMasterInventory { get; set; } = new List<Inventory>();

    public virtual MachineComponent? MachineComponents { get; set; }

    public virtual Machinery? Machinery { get; set; }

    public virtual Inventory? MasterInventory { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Warranty> Warranties { get; set; } = new List<Warranty>();
}
