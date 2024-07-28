using SAM.BusinessTier.Payload.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardStatistics> GetYearlyStatistics(int year);
        Task<CountOrders> CountAllOrde();
    }
}
