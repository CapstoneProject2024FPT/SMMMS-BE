using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Certification
{
    public Guid Id { get; set; }

    public string? CertificationLink { get; set; }

    public DateTime? DateObtained { get; set; }

    public Guid? AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
