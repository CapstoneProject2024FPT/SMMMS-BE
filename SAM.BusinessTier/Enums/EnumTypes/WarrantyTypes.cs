using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Enums.EnumStatus
{
    public enum WarrantyTypes
    {
        NoWarranty,  // Không bảo hành
        Limited,      // Bảo hành có giới hạn
        Extend,     // Bảo hành mở rộng
        LifeTime,     // Bảo hành trọn đời
        Periodic,     // Bảo hành định kỳ
        CustomerRequest // Bảo hành theo yêu cầu của khách hàng
    }

}
