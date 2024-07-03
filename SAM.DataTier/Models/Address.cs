﻿using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Address
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public string? Description { get; set; }

    public Guid? CityId { get; set; }

    public Guid? DistrictsId { get; set; }

    public Guid? WardsId { get; set; }

    public Guid? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual City? City { get; set; }

    public virtual District? Districts { get; set; }

    public virtual Ward? Wards { get; set; }
}
