using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAM.BusinessTier.Extensions;
using SAM.BusinessTier.Payload.Admin;
using SAM.BusinessTier.Services.Interfaces;
using SAM.DataTier.Models;
using SAM.DataTier.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessTier.Services.Implements
{
    public class AdminDashboardService : BaseService<AdminDashboardService>, IAdminDashboardService
    {
        public AdminDashboardService(IUnitOfWork<SamContext> unitOfWork, ILogger<AdminDashboardService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }
        public async Task<AdminDashboardStatistics> GetYearlyStatistics(int year)
        {
            var orders = await _unitOfWork.GetRepository<Order>().GetListAsync(
                predicate: o => o.CreateDate.HasValue && o.CreateDate.Value.Year == year,
                include: o => o.Include(o => o.OrderDetails));

            //var paidOrCompletedOrders = orders.Where(o => o.Status == "paid" || o.Status == "completed").ToList();
            var paidOrCompletedOrders = orders.Where(o => o.Status.Equals("paid", StringComparison.OrdinalIgnoreCase) || o.Status.Equals("completed", StringComparison.OrdinalIgnoreCase)).ToList();
            double totalCost = paidOrCompletedOrders.Sum(o => o.OrderDetails.Sum(od => (od.Quantity ?? 0) * (od?.SellingPrice ?? 0)));
            double totalRevenue = paidOrCompletedOrders.Sum(o => o.FinalAmount ?? 0);
            double totalProfit = totalRevenue - totalCost;


            return new AdminDashboardStatistics
            {
                TotalOrders = orders.Count,
                OrdersByStatus = orders.CountOrderEachStatus(),
                TotalRevenue = totalRevenue,
                TotalProfit = totalProfit
            };
        }
    }
}
