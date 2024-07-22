using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Inventory
{
    public class CreateNewInventoryRequest
    {
        public Guid? MachineryId { get; set; }

        public Guid? MachineComponentsId { get; set; }

        public Guid? MasterInventoryId { get; set; }
    }
}
