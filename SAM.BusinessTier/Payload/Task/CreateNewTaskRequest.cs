using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Task
{
    public class CreateNewTaskRequest
    {

        public string? Type { get; set; }

        public DateTime? ExcutionDate { get; set; }

        public Guid? WarrantyDetailId { get; set; }

        public Guid? OrderId { get; set; }

        public Guid AccountId { get; set; }
        public string? Note { get; set; }

    }
}
