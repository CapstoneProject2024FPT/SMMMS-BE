using System;
using System.Collections.Generic;

namespace SAM.DataTier.Models;

public partial class Notification
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsRead { get; set; }

    public string? NotificationType { get; set; }

    public Guid? AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
