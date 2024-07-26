using SAM.BusinessTier.Enums.EnumStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Payload.Admin
{
    public class AdminDashboardStatistics
    {
        public int TotalOrders { get; set; }
        public Dictionary<OrderStatus, int>? OrdersByStatus { get; set; }
        public double TotalRevenue { get; set; }
        public double TotalProfit { get; set; }
        public List<MonthlyStatistics> MonthlyStatistics { get; set; }
    }
    public class MonthlyStatistics
    {
        public int Month { get; set; }
        public int TotalOrders { get; set; }
        public double TotalRevenue { get; set; }
        public double TotalProfit { get; set; }
    }
}
