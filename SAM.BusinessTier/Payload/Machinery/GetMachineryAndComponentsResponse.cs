using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Machinery
{
    public class GetMachineryAndComponentsResponse
    {
        public GetMachinerySpecificationsRespone? Machinery { get; set; }
        public List<GetInventoryResponse>? Components { get; set; }
    }

    public class GetInventoryResponse
    {
        public Guid Id { get; set; }
        public string? SerialNumber { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? Condition { get; set; }
        public string? IsRepaired { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? SoldDate { get; set; }
        public Guid? MachineComponentsId { get; set; }
        public Guid? MasterInventoryId { get; set; }
    }
}
