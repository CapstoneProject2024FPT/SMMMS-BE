using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Enums.EnumStatus
{
    public enum WarrantyStatus
    {
        AwaitingAssignment,  // Đang đợi giao cho nhân viên
        Process,          // Đang tiến hành (chuẩn bị tới nhà)
        Completed,            // Hoàn thành
        Cancel
    }
}
