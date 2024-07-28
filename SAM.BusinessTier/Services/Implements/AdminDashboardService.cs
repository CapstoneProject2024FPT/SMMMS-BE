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

            var paidOrCompletedOrders = orders
                .Where(o => o.Status.Equals("Paid", StringComparison.OrdinalIgnoreCase) || o.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                .ToList();

            double totalCost = paidOrCompletedOrders.Sum(o => o.OrderDetails.Sum(od => (od.Quantity ?? 0) * (od?.SellingPrice ?? 0)));
            double totalRevenue = paidOrCompletedOrders.Sum(o => o.FinalAmount ?? 0);
            double totalProfit = totalRevenue - totalCost;

            var monthlyStatistics = orders
                .GroupBy(o => o.CreateDate.Value.Month)
                .Select(g => new MonthlyStatistics
                {
                    Month = g.Key,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Where(o => o.Status.Equals("Paid", StringComparison.OrdinalIgnoreCase) || o.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase)).Sum(o => o.FinalAmount ?? 0),
                    TotalProfit = g.Where(o => o.Status.Equals("Paid", StringComparison.OrdinalIgnoreCase) || o.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase)).Sum(o => o.FinalAmount ?? 0) - g.Where(o => o.Status.Equals("paid", StringComparison.OrdinalIgnoreCase) || o.Status.Equals("completed", StringComparison.OrdinalIgnoreCase)).Sum(o => o.OrderDetails.Sum(od => (od.Quantity ?? 0) * (od?.SellingPrice ?? 0)))
                })
                .ToList();

            var completeMonthlyStatistics = Enumerable.Range(1, 12)
                .Select(month => monthlyStatistics.FirstOrDefault(ms => ms.Month == month) ?? new MonthlyStatistics
                {
                    Month = month,
                    TotalOrders = 0,
                    TotalRevenue = 0,
                    TotalProfit = 0
                })
                .OrderBy(ms => ms.Month)
                .ToList();

            return new AdminDashboardStatistics
            {
                TotalOrders = orders.Count,
                OrdersByStatus = orders.CountOrderEachStatus(),
                TotalRevenue = totalRevenue,
                TotalProfit = totalProfit,
                MonthlyStatistics = completeMonthlyStatistics
            };
        }
        public async Task<CountOrders> CountAllOrde()
        {
            var orders = await _unitOfWork.GetRepository<Order>().GetListAsync(
                predicate: o => o.CreateDate.HasValue,
                include: o => o.Include(o => o.OrderDetails));

            var paidOrCompletedOrders = orders
                .Where(o => o.Status.Equals("Paid", StringComparison.OrdinalIgnoreCase) || o.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                .ToList();

            double totalCost = paidOrCompletedOrders.Sum(o => o.OrderDetails.Sum(od => (od.Quantity ?? 0) * (od?.SellingPrice ?? 0)));
            double totalRevenue = paidOrCompletedOrders.Sum(o => o.FinalAmount ?? 0);
            double totalProfit = totalRevenue - totalCost;


            return new CountOrders
            {
                TolalOrders = orders.Count,
                OrdersByStatus = orders.CountOrderEachStatus(),
                TotalRevenue = totalRevenue,
                TotalProfit = totalProfit,
            };
        }



    }
}
