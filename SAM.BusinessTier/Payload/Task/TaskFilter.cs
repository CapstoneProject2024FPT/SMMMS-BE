using SAM.BusinessTier.Enums.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Task
{
    public class TaskFilter
    {

        public TaskType? Type { get; set; }

        public DateTime? CreateDate { get; set; }

        public TaskStatus? Status { get; set; }

        public Guid? WarrantyDetailId { get; set; }

        public Guid? OrderId { get; set; }

        public Guid? AccountId { get; set; }

        public Guid? AddressId { get; set; }
    }
}
