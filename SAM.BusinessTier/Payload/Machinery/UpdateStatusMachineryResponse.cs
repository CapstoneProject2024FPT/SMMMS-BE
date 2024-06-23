using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Machinery
{
    public class UpdateStatusMachineryResponse
    {
        public MachineryStatus? Status { get; set; }

    }
}
