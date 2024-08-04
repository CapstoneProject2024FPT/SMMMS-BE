using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Address
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public Guid? CityId { get; set; }

    public Guid? DistrictId { get; set; }

    public Guid? WardId { get; set; }

    public Guid? AccountId { get; set; }

    public string? NamePersional { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual Account? Account { get; set; }

    public virtual City? City { get; set; }

    public virtual District? District { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<TaskManager> TaskManagers { get; set; } = new List<TaskManager>();

    public virtual Ward? Ward { get; set; }
}
