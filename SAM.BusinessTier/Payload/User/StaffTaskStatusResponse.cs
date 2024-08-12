using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.User
{
    public class StaffTaskStatusResponse
    {
        public Guid StaffId { get; set; }
        public string StaffName { get; set; }
        public Dictionary<TaskManagerStatus, int> TodayTaskStatusCount { get; set; }
        public Dictionary<TaskManagerStatus, int> TaskStatusCount { get; set; }
    }
    public class StaffTaskByDay
    {
        Guid StaffId { get; set; }
        string StaffName { get; set; }
        public Dictionary<TaskManagerStatus, int> TodayTaskStatusCount { get; set; }
    }
}
